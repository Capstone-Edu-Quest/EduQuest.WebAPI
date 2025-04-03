using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.CourseStatistics;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

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
        return await _context.Learners.FirstOrDefaultAsync(a => a.UserId.Equals(userId) && a.CourseId.Equals(courseId));
    }

	public async Task<List<CourseEnrollOverTime>> GetCourseEnrollOverTimeAsync(string courseId)
	{
		var query = from learner in _context.Learners
					where learner.CourseId == courseId 
					group learner by new
					{
						Year = ((DateTime)learner.UpdatedAt).Year,
						Month = ((DateTime)learner.UpdatedAt).Month
					} into g
					select new CourseEnrollOverTime
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

}
