using EduQuest_Domain.Entities;
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
	public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
	{
		private readonly ApplicationDbContext _context;

		public AnswerRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Answer>> GetListAnswerByQuestionId(string questionId)
		{
			return await _context.Answers.Where(x => x.QuestionId == questionId).ToListAsync();
		}
	}
}
