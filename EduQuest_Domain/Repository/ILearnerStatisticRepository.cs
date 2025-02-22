

using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ILearnerStatisticRepository : IGenericRepository<LearnerStatistic>
{
    // check if user had registerd or not
    Task<bool> RegisteredCourse(string courseId, string userId);
}
