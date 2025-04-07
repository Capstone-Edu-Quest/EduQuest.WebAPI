using EduQuest_Domain.Models.CourseStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Courses
{
	public class CourseStatisticForInstructor
	{
        public List<ChartInfo>? CoursesEnroll { get; set; }
        public List<CourseRatingOverTime>? CoursesReview { get; set; }
        public List<LearnerStatus>? LearnerStatus { get; set; }
        public List<TopCourseInfo>? TopCourseInfo { get; set; }
    }
}
