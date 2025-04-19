using EduQuest_Domain.Models.CourseStatistics;
using EduQuest_Domain.Models.Notification;

namespace EduQuest_Application.DTO.Response.HomeStatistic
{
	public class StatisticForInstructor
	{
        public int TotalCourses { get; set; }
        public int TotalLearners { get; set; }
        public decimal AverageReviews { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<TopCourseLearner> TopCourses { get; set; }
        public List<NotificationDto> Notification { get; set; }
    }
}
