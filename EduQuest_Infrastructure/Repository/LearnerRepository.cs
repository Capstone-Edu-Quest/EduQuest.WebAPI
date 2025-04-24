using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.CourseStatistics;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EduQuest_Infrastructure.Repository;

public class LearnerRepository : GenericRepository<CourseLearner>, ILearnerRepository
{
    private readonly ApplicationDbContext _context;

    public LearnerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<double?> TotalLearningTimeByUserId(string userId)
    {
        return await _context.Learners.AsNoTracking().Where(x => x.UserId.Equals(userId)).SumAsync(x => x.TotalTime);
    }

    public async Task<CourseLearner?> GetByUserIdAndCourseId(string userId, string courseId)
    {
        return await _context.Learners.AsNoTracking().FirstOrDefaultAsync(a => a.UserId.Equals(userId) && a.CourseId.Equals(courseId));
    }

    public async Task<List<CourseLearner>> GetByUserIdAndCourseIdsAsync(string userId, List<string> courseIds)
    {
        return await _context.Learners.AsNoTracking().Where(a => a.UserId.Equals(userId) && courseIds.Contains(a.CourseId)).ToListAsync();
    }

    public async Task<List<ChartInfo>> GetCourseEnrollOverTimeAsync(string courseId)
	{
		var now = DateTime.Now.ToUniversalTime();
		var sixMonthsAgo = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-5);

		var last6Months = Enumerable.Range(0, 6)
			.Select(i => now.AddMonths(-i))
			.Select(d => new { Year = d.Year, Month = d.Month })
			.OrderBy(d => d.Year).ThenBy(d => d.Month)
			.ToList();

		var rawData = await (from learner in _context.Learners
							 where learner.CourseId == courseId
								&& learner.UpdatedAt != null
								&& learner.UpdatedAt >= sixMonthsAgo
							 group learner by new
							 {
								 Year = learner.UpdatedAt.Value.Year,
								 Month = learner.UpdatedAt.Value.Month
							 } into g
							 select new
							 {
								 g.Key.Year,
								 g.Key.Month,
								 Count = g.Count()
							 }).ToListAsync();

		var result = last6Months.Select(m =>
		{
			var match = rawData.FirstOrDefault(x => x.Year == m.Year && x.Month == m.Month);
			return new ChartInfo
			{
				Time = $"{DateTimeHelper.GetMonthName(m.Month)} {m.Year}",
				Count = match?.Count.ToString() ?? "0"
			};
		}).ToList();

		return result;
	}

	public async Task<List<CourseLearner>> GetCoursesByUserId(string userId)
	{
		return await _context.Learners.Where(x => x.UserId == userId && x.ProgressPercentage != null && x.TotalTime >= 0 && x.IsActive == true).ToListAsync();
	}

	public async Task<List<LearnerStatus>> GetLearnerStatusAsync(List<string> courseIds)
	{
		var query = from learner in _context.Learners
					where courseIds.Contains(learner.CourseId) 
					select new
					{
						Status = learner.ProgressPercentage == 100 ? GeneralEnums.LearnerStatus.Completed.ToString() :
								 (learner.ProgressPercentage < 100 && learner.ProgressPercentage.HasValue) ? GeneralEnums.LearnerStatus.InProgress.ToString() :
								 null 
					};

		var result = await query
						  .GroupBy(x => x.Status) 
						  .Select(g => new LearnerStatus
						  {
							  Status = g.Key,
							  Count = g.Count()
						  })
						  .ToListAsync();

		return result;
	}

	public async Task<List<ChartInfo>> GetMyCoursesEnrollOverTimeAsync(List<string> courseIds)
	{
		var now = DateTime.Now.ToUniversalTime();
		var sixMonthsAgo = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-5);

		var last6Months = Enumerable.Range(0, 6)
			.Select(i => now.AddMonths(-i))
			.Select(d => new { Year = d.Year, Month = d.Month })
			.OrderBy(d => d.Year).ThenBy(d => d.Month)
			.ToList();

		var rawData = await (from learner in _context.Learners
							 where learner.UpdatedAt != null
								&& courseIds.Contains(learner.CourseId)
								&& learner.UpdatedAt >= sixMonthsAgo
							 group learner by new
							 {
								 Year = learner.UpdatedAt.Value.Year,
								 Month = learner.UpdatedAt.Value.Month
							 } into g
							 select new
							 {
								 g.Key.Year,
								 g.Key.Month,
								 Count = g.Count()
							 }).ToListAsync();

		var result = last6Months.Select(m =>
		{
			var match = rawData.FirstOrDefault(x => x.Year == m.Year && x.Month == m.Month);
			return new ChartInfo
			{
				Time = $"{DateTimeHelper.GetMonthName(m.Month)} {m.Year}",
				Count = match?.Count.ToString() ?? "0"
			};
		}).ToList();
		return result;
	}

	public async Task<List<TopCourseInfo>> GetTop3CoursesAsync(List<string> courseIds)
	{
		var query = from courseStatistic in _context.CourseStatistics
					where courseIds.Contains(courseStatistic.CourseId) && courseStatistic.DeletedAt == null
					select new
					{
						courseStatistic.CourseId,
						courseStatistic.TotalLearner,
						courseStatistic.Course,
					};

		var courseData = await query
			.Where(x => x.TotalLearner > 0)  
			.ToListAsync();

		var feedbackCounts = await _context.Feedbacks
			.Where(f => courseIds.Contains(f.CourseId))  // Filter by courseIds
			.GroupBy(f => f.CourseId)
			.Select(g => new
			{
				CourseId = g.Key,
				RatingCountThreeToFive = g.Count(x => x.Rating > 3 && x.Rating <= 5),
				RatingCountOneToThree = g.Count(x => x.Rating <= 3)
			})
			.ToListAsync();

		var top3Courses = courseData
			.GroupJoin(feedbackCounts,
				course => course.CourseId,
				feedback => feedback.CourseId,
				(course, feedbackGroup) => new TopCourseInfo
				{
					Title = course.Course.Title,
					Data = new List<int>
					{
					(int)course.TotalLearner,  
                    feedbackGroup.Any() ? feedbackGroup.First().RatingCountThreeToFive : 0,  
                    feedbackGroup.Any() ? feedbackGroup.First().RatingCountOneToThree : 0  
					}
				})
			.OrderByDescending(x => x.Data[0])  
			.Take(3)  
			.ToList();

		return top3Courses;
	}

	public async Task<bool> RegisteredCourse(string courseId, string userId)
    {
        return await _context.Learners.AnyAsync(l => l.UserId == userId && l.CourseId == courseId);
    }
    public async Task<IList<CourseLearner>> GetRecentCourseByUserId(string userId)
    {
        return await _context.Learners
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.TotalTime) 
            .Take(5)  
            .ToListAsync();
    }

    public async Task<int> CountNumberOfCourseByUserId(string userId)
    {
        return await _context.Learners.AsNoTracking().Where(a => a.UserId.Equals(userId)).CountAsync();
    }

	public async Task<List<string>> GetCoursesIdStudying(string userId)
	{
		var list = await _context.Learners.Where(x => x.UserId == userId).ToListAsync();
		return list.Select(x => x.CourseId).Distinct().ToList();
	}

    public async Task<List<CourseLearner>> GetFinishedLearner()
    {
        return await _context.Learners.AsNoTracking().Where(x => x.ProgressPercentage >= 90).ToListAsync();
    }

	public async Task<List<CourseLearner>> GetListLearnerOfCourse(string courseId)
	{
		return await _context.Learners.Where(x => x.CourseId == courseId).ToListAsync();
	}

	public async Task<List<TopCourseLearner>> GetTopCourseLearner(List<string> courseIds)
	{
		var query = from learner in _context.Learners
					join courseStatistic in _context.CourseStatistics
						on learner.CourseId equals courseStatistic.CourseId
					where courseIds.Contains(learner.CourseId)
					group new { learner, courseStatistic } by learner.CourseId into g
					select new TopCourseLearner
					{
						Title = g.FirstOrDefault().courseStatistic.Course.Title,
						LearnerCount = g.Count()
					};

		var topCourses = await query
								.OrderByDescending(x => x.LearnerCount).ToListAsync();
		return topCourses;
	}
}
