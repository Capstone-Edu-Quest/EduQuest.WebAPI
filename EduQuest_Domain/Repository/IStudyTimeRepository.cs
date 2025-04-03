using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IStudyTimeRepository : IGenericRepository<StudyTime>
{
    Task<IList<StudyTime>> GetStudyTimeByUserId(string userId);
}
