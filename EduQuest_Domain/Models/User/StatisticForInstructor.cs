using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Models.User
{
	public class StatisticForInstructor
	{
        public int TotalCourses { get; set; }
        public int TotalLearners { get; set; }
        public decimal AverageReviews { get; set; }
        public decimal AverageRevenue { get; set; }
        public List<string> TopCourses { get; set; }
    }
}
