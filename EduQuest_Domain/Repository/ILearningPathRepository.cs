using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
namespace EduQuest_Domain.Repository;

public interface ILearningPathRepository: IGenericRepository<LearningPath>
{
    Task<List<LearningPath>> GetMyLearningPaths(string UserId);
    Task<List<LearningPath>> GetMyPublicLearningPaths(string UserId);
    //Task<LearningPath> CreateLearningPath(CreateLearningPathRequest entity);
}
