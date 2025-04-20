using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IQuizAttemptRepository: IGenericRepository<QuizAttempt>
{
    Task<int> GetAttemptNo(string quizId, string lessonId, string userId);
    Task<List<QuizAttempt>> GetQuizAttempts(string quizId, string lessonId, string userId);
    Task<List<QuizAttempt>> GetQuizzesAttempts(List<string> quizId, List<string> lessonId, string userId);
}
