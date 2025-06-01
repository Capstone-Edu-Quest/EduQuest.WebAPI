using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
	public interface IAssignmentRepository : IGenericRepository<Assignment>
	{
		Task<List<Assignment>> GetByUserId(string userId, SearchLessonContent? info);
	}
}
