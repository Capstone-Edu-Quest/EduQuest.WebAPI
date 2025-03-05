using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ILearnerRepository : IGenericRepository<CourseLearner>
{
    Task<CourseLearner?> GetByUserIdAndCourseId(string userId, string courseId);
    Task<bool> RegisteredCourse(string courseId, string userId);
}
