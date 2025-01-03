using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Item")]
	public partial class Item : BaseEntity
	{
		public string Name { get; set; }
		public int Price { get; set; }
		public string Description { get; set; }


		[JsonIgnore]
		public virtual ICollection<Course> Courses { get; set; }
	}
}
