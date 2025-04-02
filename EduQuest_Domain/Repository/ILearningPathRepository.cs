using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;
namespace EduQuest_Domain.Repository;

public interface ILearningPathRepository: IGenericRepository<LearningPath>
{
    Task<List<LearningPath>> GetMyLearningPaths(string UserId);
    Task<PagedList<LearningPath>> GetMyLearningPaths(string UserId, string? keyWord, string? type, int page, int eachPage);
    Task<List<LearningPath>> GetMyPublicLearningPaths(string UserId);
    Task<LearningPath?> GetLearningPathDetail(string LearningPathId);
    Task<List<Course>> GetLearningPathCourse(string learningPathId);
    Task<bool> IsOwner (string UserId, string learningPathId);
    Task<LearningPath> GetMySpecificLearningPath(string userId, string learningId);
}
