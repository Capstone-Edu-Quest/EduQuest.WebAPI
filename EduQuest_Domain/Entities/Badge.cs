using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Badge")]
	public partial class Badge : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string IconUrl { get; set; }
		public string Color { get; set; }

		[JsonIgnore]
		public virtual ICollection<User> Users { get; set; }
	}
}
