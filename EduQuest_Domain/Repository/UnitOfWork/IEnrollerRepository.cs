using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository.UnitOfWork;

public interface IEnrollerRepository : IGenericRepository<Enroller>
{
    Task<List<Enroller>?> GetByLearningPathId(string learningPathId, string userId);
    Task<List<Enroller>?> GetByLearningPathId(string learningPathId);
    Task<List<Enroller>?> GetByCourseId(string learningPathId, string courseId);
    Task<List<Enroller>?> GetEnrollerIds(string learningPathId);
    Task<int> GetTotalLearningDay(string learningPathId, string userId);
}
