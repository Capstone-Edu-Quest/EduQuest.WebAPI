using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.UserStatistics;


namespace EduQuest_Application.DTO.Response;

public class AdminDashboard
{
    public AdminDasboardUsers AdminDasboardUsers { get; set; } = new AdminDasboardUsers();
    public AdminDashboardCourses AdminDashboardCourses { get; set; } = new AdminDashboardCourses();
    public int PendingViolations { get; set; } = 0;
    
}
