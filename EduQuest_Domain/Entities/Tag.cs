using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Tag")]
	public partial class Tag: BaseEntity
	{
		public string Name { get; set; }

		[JsonIgnore]
		public virtual ICollection<Course> Courses { get; set; }
	}
}
