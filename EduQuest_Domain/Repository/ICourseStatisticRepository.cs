using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.User;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
	public interface ICourseStatisticRepository : IGenericRepository<CourseStatistic>
	{
		Task<CourseStatistic> GetByCourseId(string courseId);
		Task<(int totalLearner, decimal avgRating)> GetTotalLearnerForInstructor(List<string> courseIds);
	}
}
