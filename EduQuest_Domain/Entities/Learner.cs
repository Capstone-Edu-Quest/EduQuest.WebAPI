using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Learner")]
	public class Learner : BaseEntity
	{
		public string UserId { get; set; }
		public string CourseId { get; set; }
        public bool IsActive { get; set; }
		public int ProgressPercentage { get; set; }
		public int Xp { get; set; }
		public int Gold { get; set; }

		public virtual User User { get; set; } = null;
		public virtual Course Course { get; set; } = null;

		[JsonIgnore]
		public virtual ICollection<Feedback>? Feedbacks { get; set; }
	}
}
