using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace EduQuest_Infrastructure.Repository;

public class LearnerRepository : GenericRepository<Learner>, ILearnerRepository
{
    private readonly ApplicationDbContext _context;

    public LearnerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Learner?> GetByUserIdAndCourseId(string userId, string courseId)
    {
        return await _context.Learners.FirstOrDefaultAsync(a => a.UserId.Equals(userId) && a.CourseId.Equals(courseId));
    }
}
