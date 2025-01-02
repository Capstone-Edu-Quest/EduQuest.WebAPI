using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("UserStatistic")]
	public partial class UserStatistic : BaseEntity
	{
		public string UserId { get; set; }
		public int TotalActiveDay { get; set; }
		public int MaxStudyStreakDay { get; set; }
		public DateTime LastLearningDay { get; set; }
		public int CompletedCourses { get; set; }

		public virtual User User { get; set; } = null!;
	}
}
