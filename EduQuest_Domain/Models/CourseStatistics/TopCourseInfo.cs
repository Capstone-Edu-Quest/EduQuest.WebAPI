using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Models.CourseStatistics
{
	public class TopCourseInfo
	{
		public string Ttile { get; set; } 
		public double RatingCount { get; set; } // Lượt rating
		public int LearnerCount { get; set; }
	}
}
