using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.CourseStatistics;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ILearnerRepository : IGenericRepository<CourseLearner>
{
    Task<List<CourseLearner>> GetFinishedLearner();
    Task<List<CourseLearner>> GetByUserIdAndCourseIdsAsync(string userId, List<string> courseIds);
    Task<CourseLearner?> GetByUserIdAndCourseId(string userId, string courseId);
    Task<bool> RegisteredCourse(string courseId, string userId);
    Task<List<ChartInfo>> GetCourseEnrollOverTimeAsync(string courseId);
    Task<List<CourseLearner>> GetCoursesByUserId(string userId);
    Task<List<ChartInfo>> GetMyCoursesEnrollOverTimeAsync(List<string> courseIds);
    Task<List<LearnerStatus>> GetLearnerStatusAsync(List<string> courseIds);
    Task<List<TopCourseInfo>> GetTop3CoursesAsync(List<string> courseIds);
    Task<IList<CourseLearner>> GetRecentCourseByUserId(string userId);
    Task<int> CountNumberOfCourseByUserId(string userId);
    Task<List<string>> GetCoursesIdStudying(string userId);
    Task<List<CourseLearner>> GetListLearnerOfCourse(string courseId);
    Task<List<TopCourseLearner>> GetTopCourseLearner(List<string> courseIds);
    
}
