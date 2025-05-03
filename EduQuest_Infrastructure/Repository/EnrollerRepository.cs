using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class EnrollerRepository : GenericRepository<Enroller>, IEnrollerRepository
{
    private readonly ApplicationDbContext _context;

    public EnrollerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<List<Enroller>?> GetByLearningPathId(string learningPathId, string userId)
    {
        return await _context.Enrollers.Where(e => e.LearningPathId == learningPathId && e.UserId == userId)
            .ToListAsync();
    }
    public async Task<List<Enroller>?> GetByLearningPathId(string learningPathId)
    {
        return await _context.Enrollers.Where(e => e.LearningPathId == learningPathId)
            .ToListAsync();
    }

    public async Task<List<Enroller>?> GetByCourseId(string learningPathId, string courseId)
    {
        return await _context.Enrollers.Where(e => e.LearningPathId == learningPathId && e.CourseId == courseId)
            .ToListAsync();
    }
    public async Task<List<Enroller>?> GetEnrollerIds(string learningPathId)
    {
        var result = await _context.Enrollers
         .Where(e => e.LearningPathId == learningPathId && e.CourseOrder == 0)
         .GroupBy(e => e.UserId)
         .Select(g => g.FirstOrDefault())
         .ToListAsync();
        return result;
    }
    public async Task<int> GetTotalLearningDay(string learningPathId, string userId)
    {
        var result = await _context.Enrollers
         .Where(e => e.LearningPathId == learningPathId && e.UserId == userId)
         .OrderBy(e =>e.DueDate)
         .ToListAsync();

        if (!result.Any())
        {
            return 0;
        }

        var firstDate = result.First().DueDate;
        var lastDate = result.Last().DueDate;

        var response = lastDate - firstDate;
        return (int)Math.Ceiling(response.Value.TotalDays);
    }
}
