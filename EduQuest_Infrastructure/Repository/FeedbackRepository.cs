using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.CourseStatistics;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Nest;

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
        var result = _context.Feedbacks.Where(f => f.CourseId == courseId);

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
		var now = DateTime.Now.ToUniversalTime();
		var sixMonthsAgo = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-5);

		var last6Months = Enumerable.Range(0, 6)
			.Select(i => now.AddMonths(-i))
			.Select(d => new { Year = d.Year, Month = d.Month })
			.OrderBy(d => d.Year).ThenBy(d => d.Month)
			.ToList();

		var rawData = await (from feedback in _context.Feedbacks
							 where feedback.CourseId == courseId
								&& feedback.UpdatedAt != null
								&& feedback.UpdatedAt >= sixMonthsAgo
							 group feedback by new
							 {
								 Year = feedback.UpdatedAt.Value.Year,
								 Month = feedback.UpdatedAt.Value.Month
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
	public async Task<List<Feedback>> GetAllByCourseId(string courseId)
    {
		return await _context.Feedbacks.Where(f => f.CourseId == courseId).ToListAsync();
    }
}
