using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository
{
	public class QuizRepository : GenericRepository<Quiz>, IQuizRepository
	{
		private readonly ApplicationDbContext _context;

		public QuizRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Quiz>> GetByUserId(string userId)
		{
			return await _context.Quizzes.Where(x => x.UserId == userId).ToListAsync();
		}

		public async Task<Quiz> GetQuizById(string quizId)
		{
			return await _context.Quizzes.Include(x => x.Questions).FirstOrDefaultAsync(x => x.Id == quizId);
		}
	}
}
