using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.Repository;

public class AssignmentAttemptRepository : GenericRepository<AssignmentAttempt>, IAssignmentAttemptRepository
{
    private readonly ApplicationDbContext _context;
    public AssignmentAttemptRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<int> GetAttemptNo(string quizId, string lessonId)
    {
        return await _context.AssignmentAttempts.Where(q => q.AssignmentId == quizId && q.LessonId == lessonId).CountAsync();
    }
    public async Task<AssignmentAttempt?> GetLearnerAttempt(string assignmentId, string userId)
    {
        return await _context.AssignmentAttempts
            .Where(q => q.AssignmentId == assignmentId && q.UserId == userId && q.AttemptNo == 1)
            .FirstOrDefaultAsync();
    }
}
