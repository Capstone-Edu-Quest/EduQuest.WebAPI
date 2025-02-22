using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class LearnerStatisticRepository : GenericRepository<LearnerStatistic>, ILearnerStatisticRepository
{
    private readonly ApplicationDbContext _context;
    public LearnerStatisticRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<bool> RegisteredCourse(string courseId, string userId)
    {
        return await _context.LearnerStatistics.AnyAsync(l => l.UserId == userId && l.CourseId == courseId);
    }
}
