using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("Setting")]
	public partial class Setting: BaseEntity
	{
		
		public int MaxStudent { get; set; }
		public string DifficultyLevel { get; set; }

		public virtual ICollection<Course> Courses { get; set; } = null;
	}
}
