using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("LearnerStatistic")]
	public partial class LearnerStatistic 
	{
		public string CourseId { get; set; }
		public string UserId { get; set; }
		public int ProgressPercentage { get; set; }
		public int Xp { get; set; }
		public int Gold { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }

		public DateTime? DeletedAt { get; set; }

		public virtual User User { get; set; } = null;
		public virtual Course Course { get; set; } = null;
	}
}
