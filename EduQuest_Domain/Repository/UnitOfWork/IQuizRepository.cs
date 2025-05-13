using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Repository.UnitOfWork
{
	public interface IQuizRepository : IGenericRepository<Quiz>
	{
		Task<Quiz> GetQuizById(string quizId);
		Task<List<Quiz>> GetByUserId(string userId);
	}
}
