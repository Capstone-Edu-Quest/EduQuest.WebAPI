using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.CourseStatistics;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ILearnerRepository : IGenericRepository<CourseLearner>
{
    Task<CourseLearner?> GetByUserIdAndCourseId(string userId, string courseId);
    Task<bool> RegisteredCourse(string courseId, string userId);
    Task<List<CourseEnrollOverTime>> GetCourseEnrollOverTimeAsync(string courseId);
    Task<List<CourseLearner>> GetCoursesByUserId(string userId);

}
