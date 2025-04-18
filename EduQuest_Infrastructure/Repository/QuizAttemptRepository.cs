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

public class QuizAttemptRepository : GenericRepository<QuizAttempt>, IQuizAttemptRepository
{
    private readonly ApplicationDbContext _context;
    public QuizAttemptRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<int> GetAttemptNo(string quizId, string lessonId)
    {
        return await _context.QuizAttempts.Where(q => q.QuizId == quizId && q.LessonId == lessonId).CountAsync();
    }
    public async Task<List<QuizAttempt>> GetQuizAttempts(string quizId, string lessonId, string userId)
    {
        return await _context.QuizAttempts
            .Where(q => q.QuizId == quizId && q.LessonId == lessonId && q.UserId == userId)
            .ToListAsync();
    }

	public async Task<List<QuizAttempt>> GetQuizzesAttempts(List<string> quizIds, List<string> lessonIds, string userId)
	{
        return await _context.QuizAttempts.Where(x => quizIds.Contains(x.QuizId) && lessonIds.Contains(x.LessonId) && x.UserId == userId).ToListAsync();
	}
}
