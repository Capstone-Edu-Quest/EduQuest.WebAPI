using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("Feedback")]
	public partial class Feedback : BaseEntity
	{
		public string UserId { get; set; }
		public string CourseId { get; set; }
		public int Rating { get; set; }
		public string Comment { get; set; }
		

		public virtual User User { get; set; } = null;
		public virtual Course Course { get; set; } = null;
	}
}
