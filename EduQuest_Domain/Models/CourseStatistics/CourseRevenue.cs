using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Models.CourseStatistics
{
	public class CourseRevenue
	{
        public string Title { get; set; }
        public int? TotalSales { get; set; }
        public decimal? TotalRefund { get; set; }
        public decimal? TotalRevenue { get; set; }
    }
}
