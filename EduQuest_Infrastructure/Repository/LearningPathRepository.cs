using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            .FirstOrDefaultAsync(l => l.Id == LearningPathId);
        return result;
    }

    public async Task<List<LearningPath>> GetMyLearningPaths(string UserId)
    {
        var result = _context.LearningPaths.Include(l => l.User).Include(l => l.LearningPathCourses)
            .Where(l => l.UserId.Equals(UserId));
        return await result.ToListAsync();
    }

    public async Task<PagedList<LearningPath>> GetMyLearningPaths(string UserId, string? keyWord, bool? isPulic, bool? isEnrolled,
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
        if (isEnrolled.HasValue && isEnrolled.Value == true)
        {
            result = from r in result
                     where r.IsEnrolled == isEnrolled
                     select r;
            /*var learningPathId = _context.Enrollers.Where(e => e.UserId == UserId).FirstOrDefault()?.LearningPathId;
            var result2 = _context.LearningPaths.Where(l => l.Id == learningPathId);
            return await result2.Pagination(page, eachPage).ToPagedListAsync(page, eachPage);*/
        }
        if (CreatedByExpert.HasValue)
        {
            result = from r in result
                     where r.CreatedByExpert == CreatedByExpert
                     select r;
        }

        var response = await result.Pagination(page, eachPage).ToPagedListAsync(page, eachPage);
        return response;
    }

    public async Task<List<LearningPath>> GetMyPublicLearningPaths(string? UserId, string? keyWord)
    {
        var result = _context.LearningPaths.Include(l => l.User).Include(l => l.LearningPathCourses)
            .Where(l => l.IsPublic == true);
        if (!string.IsNullOrEmpty(UserId))
        {
            result = from r in result
                     where r.UserId == UserId
                     select r;
        }
        if (!string.IsNullOrEmpty(keyWord))
        {
            result = from r in result
                     where (r.Name.Contains(keyWord) || r.Description.Contains(keyWord))
                     select r;
        }
        return await result.ToListAsync();
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

    public async Task<LearningPath> GetMySpecificLearningPath(string userId, string learningId)
    {
        return await _context.LearningPaths.Include(x => x.LearningPathCourses).FirstOrDefaultAsync(x => x.UserId == userId && x.Id == learningId);
    }

    public async Task<LearningPath?> EnrollLearningPath(string learningPathId, string userId)
    {
        var learningPath = await _context.LearningPaths
            .FirstOrDefaultAsync(l => l.Id == learningPathId);

        if (learningPath == null)
        {
            return null;
        }
        learningPath.IsEnrolled = true;

        await _context.LearningPaths
            .Where(l => l.Id != learningPathId && l.UserId == userId)
            .ExecuteUpdateAsync(q => q.SetProperty(l => l.IsEnrolled, false));

        var learningPathIds = await _context.LearningPaths
            .Where(l => l.Id != learningPathId && l.UserId == userId)
            .Select(l => l.Id).ToListAsync();

        var removeDueDates = await _context.LearningPathCourses
            .Where(l => learningPathIds.Contains(l.LearningPathId))
            .ToListAsync();

        foreach (var course in removeDueDates)
        {
            course.DueDate = null;
            course.IsOverDue = false;
        }

        await _context.SaveChangesAsync();
        return learningPath;
    }

    public async Task<int> UpdateLearningPathCourseDueDate()
    {
        return await _context.LearningPathCourses
            .Where(l => l.DueDate != null && l.DueDate!.Value.Date <= DateTime.Now.Date.ToUniversalTime()
            && l.IsCompleted == false)
            .ExecuteUpdateAsync(q => q
            .SetProperty(l => l.IsOverDue, true));
    }
}
