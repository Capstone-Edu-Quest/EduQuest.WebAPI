using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class LearningPathRepository : GenericRepository<LearningPath>, ILearningPathRepository
{
    private readonly ApplicationDbContext _context;

    public LearningPathRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<LearningPath?> GetLearningPathDetail(string LearningPathId)
    {
        var result = await _context.LearningPaths
            .Include(l => l.Tags)
            .Include(l => l.LearningPathCourses)
            .Include(l => l.User)
            .FirstOrDefaultAsync(l=> l.Id == LearningPathId);
        return result;
    }

    public async Task<List<LearningPath>> GetMyLearningPaths(string UserId)
    {
        var result = _context.LearningPaths.Include(l => l.User).Include(l => l.LearningPathCourses)
            .Where(l => l.UserId.Equals(UserId));
        return await result.ToListAsync();
    }

    public Task<PagedList<LearningPath>> GetMyLearningPaths(string UserId, string? keyWord, bool? isPulic, bool? isEnrolled,
        bool? CreatedByExpert, int page, int eachPage)
    {
        var result = _context.LearningPaths.Include(l => l.User).Include(l => l.LearningPathCourses)
            .Where(l => l.UserId.Equals(UserId));

        if (!string.IsNullOrEmpty(keyWord))
        {
            result = from r in result
                     where (r.Name.Contains(keyWord) || r.Description.Contains(keyWord))
                     select r;
        }

        if (isPulic.HasValue)
        {
            result = from r in result
                     where r.IsPublic == isPulic
                     select r;
        }
        if(isEnrolled.HasValue)
        {
            result = from r in result
                     where r.IsEnrolled == isEnrolled
                     select r;
        }
        if(CreatedByExpert.HasValue)
        {
            result = from r in result
                     where r.CreatedByExpert == CreatedByExpert
                     select r;
        }
        
        var response = result.Pagination(page, eachPage).ToPagedListAsync(page, eachPage);
        return response;
    }

    public async Task<List<LearningPath>> GetMyPublicLearningPaths(string UserId)
    {
        var result = await _context.LearningPaths.Include(l => l.User).Include(l => l.LearningPathCourses)
            .Where(l => l.UserId.Equals(UserId) && l.IsPublic == true).ToListAsync();
        return result;
    }
    public async Task<List<Course>> GetLearningPathCourse(string learningPathId)
    {
        return await _context.LearningPathCourses
        .Where(l => l.LearningPathId == learningPathId)
        .Include(l => l.Course.CourseStatistic).Include(l => l.Course.User)
        .Select(l => l.Course)
        .ToListAsync();
    }

    public async Task<bool> IsOwner(string UserId, string learningPathId)
    {
        var result = await _context.LearningPaths.FindAsync(learningPathId);
        return UserId == result!.UserId ? true : false;
    }
}
