using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository.UnitOfWork
{
	public interface IQuizRepository : IGenericRepository<Quiz>
	{
		Task<Quiz> GetQuizById(string quizId);
		Task<List<Quiz>> GetByUserId(string userId, SearchLessonContent info);
	}
}
