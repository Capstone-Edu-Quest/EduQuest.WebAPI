using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("Setting")]
	public partial class Setting: BaseEntity
	{
		public string UserId { get; set; }
		public int MaxStudent { get; set; }
		public string DifficultyLevel { get; set; }

		public virtual User User { get; set; } = null;
	}
}
