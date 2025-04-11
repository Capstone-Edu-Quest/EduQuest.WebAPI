using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Request;
using EduQuest_Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Repository
{
	public interface ICourseRepository : IGenericRepository<Course>
	{
		Task<(List<Course> Courses, int TotalItems)> SearchCoursesAsync(SearchCourseRequestDto request, int pageNo, int eachPage);

        Task UpdateStatus(string status, string courseId);
        Task<Course> GetCourseById(string Id);
		Task<List<Course>> GetCourseByUserId(string Id);
		//Task<IEnumerable<Course>> GetAllCourse();
		Task<int> GetTotalTime(string courseId);
		Task<bool> IsExist(string courseId);
		Task<List<Course>> GetCoursesByKeywordsAsync(List<string> keywords);
        Task<bool> IsOwner(string courseId, string UserId);
		Task<List<Tag>> GetTagByCourseIds(List<string> courseIds);
		Task<List<Course>?> GetByListIds(List<string> courseIds);
		Task<List<Course>> GetListCourse();
		Task<Course> GetCourseLearnerByCourseId(string courseId);
		Task<IEnumerable<Course>> GetCourseByStatus(string status);
		Task<Course> GetCourseByUserIdAndCourseId(string userId, string courseId);
		Task<int> GetCourseCountByCourseIdAsync(string courseId);
		Task<List<Course>> GetCoursesByInstructorIdAsync(string instructorId);
		Task<Course> GetCourseWithLessonsAndMaterialsAsync(string courseId);
		Task<List<Course>> GetCoursesByAssignToAsync(string expertId);

        //admin dashboard
        Task<AdminDashboardCourses> GetAdminDashBoardStatistic();

    }
}
