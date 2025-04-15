using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("Certificate")]
	public partial class Certificate : BaseEntity
	{
		public string Title { get; set; } = string.Empty;
		public string? Url { get; set; }
        public string? UserId { get; set; }
        public string CourseId { get; set; }

		public virtual User Users { get; set; }
		public virtual Course Course { get; set; } = null!;
	}
}
