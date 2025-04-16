using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.CourseStatistics;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
{
    private readonly ApplicationDbContext _context;
    public FeedbackRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

	

	public async Task<PagedList<Feedback>> GetByCourseId(string courseId, int pageNo, int pageSize, int? rating, string? feedback)
    {
        var result = _context.Feedbacks.Include(f => f.User).Where(f => f.CourseId == courseId);

        if (rating.HasValue)
        {
            result = from r in result
                     where r.Rating == rating.Value
                     select r;
        }
        if(!string.IsNullOrWhiteSpace(feedback))
        {
            result = from r in result 
                     where r.Comment.Contains(feedback!)
                     select r;
        }

        return await result.Pagination(pageNo, pageSize).ToPagedListAsync(pageNo, pageSize);
    }

	public async Task<List<ChartInfo>> GetCourseRatingOverTimeAsync(string courseId)
	{
		var query = from feedback in _context.Feedbacks
					where feedback.CourseId == courseId
					group feedback by new
					{
						Year = ((DateTime)feedback.UpdatedAt).Year,
						Month = ((DateTime)feedback.UpdatedAt).Month
					} into g
					select new ChartInfo
					{
						Time = $"{DateTimeHelper.GetMonthName(g.Key.Month)} {g.Key.Year}",
						Count = g.Count().ToString()
					};

		return await query.ToListAsync();
	}

	public async Task<List<CourseRatingOverTime>> GetMyCoursesRatingOverTimeAsync(List<string> courseIds)
	{
		var query = from feedback in _context.Feedbacks
					where courseIds.Contains(feedback.CourseId)  
					group feedback by feedback.Rating into g 
					select new CourseRatingOverTime
					{
						Rating = g.Key,  
						Count = g.Count()  
					};

		var result = await query.ToListAsync();

		var courseRatingList = new List<CourseRatingOverTime>();

		for (int rating = 1; rating <= 5; rating++)
		{
			var existingRating = result.FirstOrDefault(r => r.Rating == rating);
			courseRatingList.Add(new CourseRatingOverTime
			{
				Rating = rating,
				Count = existingRating?.Count ?? 0 
			});
		}

		return courseRatingList;
	}

	public async Task<bool> IsOnwer(string feedbackId, string UserId)
    {
        var result = await _context.Feedbacks.FindAsync(feedbackId);
        return UserId == result!.UserId ? true : false;
    }
}
