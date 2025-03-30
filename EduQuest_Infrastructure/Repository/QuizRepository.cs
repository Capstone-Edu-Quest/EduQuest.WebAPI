using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.Repository
{
	public class QuizRepository : GenericRepository<Quiz>, IQuizRepository
	{
		private readonly ApplicationDbContext _context;

		public QuizRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Quiz> GetQuizById(string quizId)
		{
			return await _context.Quizzes.Include(x => x.Questions).FirstOrDefaultAsync(x => x.Id == quizId);
		}
	}
}
