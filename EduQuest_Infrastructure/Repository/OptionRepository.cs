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

namespace EduQuest_Infrastructure.Repository
{
    public class OptionRepository : GenericRepository<Option>, IOptionRepository
	{
		private readonly ApplicationDbContext _context;

		public OptionRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Option>> GetListAnswerByQuestionId(string questionId)
		{
			return await _context.Answers.Where(x => x.QuestionId == questionId).ToListAsync();
		}
	}
}
