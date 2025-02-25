using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ILearnerRepository : IGenericRepository<Learner>
{
    Task<Learner?> GetByUserIdAndCourseId(string userId, string courseId);
}
