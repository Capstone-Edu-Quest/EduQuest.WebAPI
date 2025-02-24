using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("CourseStatistic")]
	public partial class CourseStatistic : BaseEntity
	{
        public string CourseId { get; set; }
        public int? TotalLesson { get; set; }
		public int? TotalTime { get; set; }
        public int? TotalLearner { get; set; }
		public double? Rating { get; set; }
        public int? TotalReview { get; set; }
		public double? TotalRevenue { get; set; }
		public double? TotalRefund { get; set; }

		public virtual Course Course { get; set; } = null!;
	}
}
