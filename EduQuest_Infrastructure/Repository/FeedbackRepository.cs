using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

     public async Task<bool> IsOnwer(string feedbackId, string UserId)
    {
        var result = await _context.Feedbacks.FindAsync(feedbackId);
        return UserId == result!.UserId ? true : false;
    }
}
