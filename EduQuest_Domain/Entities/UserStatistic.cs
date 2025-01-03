using System.ComponentModel.DataAnnotations.Schema;

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
