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
		var query = from learner in _context.Learners
					where learner.CourseId == courseId 
					group learner by new
					{
						Year = learner.UpdatedAt.Value.Year,
						Month = learner.UpdatedAt.Value.Month
					} into g
					select new ChartInfo
					{
						Time = $"{DateTimeHelper.GetMonthName(g.Key.Month)} {g.Key.Year}",
						Count = g.Count().ToString() 
					};

		return await query.ToListAsync();
	}

	public async Task<List<CourseLearner>> GetCoursesByUserId(string userId)
	{
		return await _context.Learners.Where(x => x.UserId == userId && x.ProgressPercentage != null && x.TotalTime >= 0).ToListAsync();
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
		var query = from learner in _context.Learners
					where courseIds.Contains(learner.CourseId)
					group learner by new
					{
						Year = learner.UpdatedAt.Value.Year, 
						Month = learner.UpdatedAt.Value.Month
					} into g
					select new ChartInfo
					{
						Time = $"{DateTimeHelper.GetMonthName(g.Key.Month)} {g.Key.Year}",
						Count = g.Count().ToString() 
					};

		return await query.ToListAsync();
	}

	public async Task<List<TopCourseInfo>> GetTop3CoursesAsync(List<string> courseIds)
	{
		var query = from learner in _context.Learners
					join courseStatistic in _context.CourseStatistics
						on learner.CourseId equals courseStatistic.CourseId
					where courseIds.Contains(learner.CourseId)
					group new { learner, courseStatistic } by learner.CourseId into g
					select new TopCourseInfo
					{
						Title = g.FirstOrDefault().courseStatistic.Course.Title,
						RatingCountOneToThree = g.Count(x => x.courseStatistic.Rating <= 3),
						RatingCountThreeToFive = g.Count(x => x.courseStatistic.Rating > 3 && x.courseStatistic.Rating <= 5),
						LearnerCount = g.Count()
					};

		var top3Courses = await query
								.OrderByDescending(x => x.LearnerCount) 
								.Take(3) 
								.ToListAsync();

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
}
