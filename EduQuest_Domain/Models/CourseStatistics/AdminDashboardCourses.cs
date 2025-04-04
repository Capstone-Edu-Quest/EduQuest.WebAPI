using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Courses;

public class AdminDashboardCourses
{
    public int TotalCourses { get; set; } = 0;
    public int NewCoursesThisMonth { get; set; } = 0;
    public string MostPopularCategory { get; set; } = string.Empty;
}
