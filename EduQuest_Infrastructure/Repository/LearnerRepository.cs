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
		var query = from courseStatistic in _context.CourseStatistics
					join feedback in _context.Feedbacks
						on courseStatistic.CourseId equals feedback.CourseId  
					where courseIds.Contains(courseStatistic.CourseId)
						  && courseStatistic.DeletedAt == null
						  && feedback.DeletedAt == null 
					group new { courseStatistic, feedback } by courseStatistic.CourseId into g
					select new
					{
						CourseId = g.Key,
						CourseStatistic = g.Select(x => x.courseStatistic).FirstOrDefault(),
						Feedbacks = g.Select(x => x.feedback).ToList() 
					};

		var courseData = await query.ToListAsync();

		var top3Courses = courseData
			.Select(g => new TopCourseInfo
			{
				Title = g.CourseStatistic?.Course.Title,  

				Data = new List<int>
				{
				(int)g.CourseStatistic.TotalLearner, 
				g.Feedbacks.Count(x => x.Rating > 3 && x.Rating <= 5),  // RatingCountThreeToFive từ bảng Feedback
				g.Feedbacks.Count(x => x.Rating <= 3) // RatingCountOneToThree từ bảng Feedback
				}
			})
			.OrderByDescending(x => x.Data[0])  // Sắp xếp theo LearnerCount
			.Take(3)  // Lấy top 3 khóa học
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
