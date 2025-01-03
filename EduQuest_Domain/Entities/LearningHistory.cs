using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("LearningHistory")]
	public partial class LearningHistory : BaseEntity
	{
		public string UserId { get; set; }
		public string CourseId { get; set; }
		public DateTime LastAccessed { get; set; }

		public virtual User User { get; set; } = null;
		public virtual Course Course { get; set; } = null;
	}
}
