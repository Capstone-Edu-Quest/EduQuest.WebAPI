using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
	public interface IQuestRepository : IGenericRepository<Quest>
	{
		Task<Quest> GetQuestById(string Id);
	}
}
