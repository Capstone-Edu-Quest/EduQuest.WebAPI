using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
	public interface IOptionRepository : IGenericRepository<Option>
    {
        Task<List<Option>> GetListAnswerByQuestionId(string questionId);
    }
}
