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
}
