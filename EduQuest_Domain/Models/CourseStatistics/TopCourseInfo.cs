using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Models.CourseStatistics
{
	public class TopCourseInfo
	{
		public string Title { get; set; } 
		public int RatingCountOneToThree { get; set; } 
		public int RatingCountThreeToFive { get; set; } 
		public int LearnerCount { get; set; }
	}
}
