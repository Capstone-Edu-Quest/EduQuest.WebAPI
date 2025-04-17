using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IAssignmentAttemptRepository : IGenericRepository<AssignmentAttempt>
{
    Task<int> GetAttemptNo(string quizId, string lessonId, string userId);
    Task<AssignmentAttempt?> GetLearnerAttempt(string lessonId, string assignmentId, string userId);
    Task<List<AssignmentAttempt>> GetLearnerAttempts(string lessonId, string assignmentId);
}
