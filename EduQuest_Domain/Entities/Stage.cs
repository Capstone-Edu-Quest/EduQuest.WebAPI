using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Stage")]
	public partial class Stage : BaseEntity
	{
		public string CourseId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Level { get; set; }

		public virtual Course Course { get; set; } = null;
		public virtual ICollection<Reward> Rewards { get; set; } = null;
		[JsonIgnore]
		public virtual LearningMaterial LearningMaterial { get; set; }
		
	}
}
