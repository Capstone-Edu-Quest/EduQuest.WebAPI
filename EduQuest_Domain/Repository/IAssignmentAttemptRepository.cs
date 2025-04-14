using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Repository;

public interface IAssignmentAttemptRepository : IGenericRepository<AssignmentAttempt>
{
    Task<int> GetAttemptNo(string quizId, string lessonId);
    Task<AssignmentAttempt?> GetLearnerAttempt(string lessonId, string assignmentId, string userId);
}
