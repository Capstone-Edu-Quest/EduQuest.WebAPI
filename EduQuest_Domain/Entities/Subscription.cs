using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Subscription")]
	public partial class Subscription : BaseEntity
	{
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public int DurationDays { get; set; }
		public decimal Price { get; set; }
		public bool IsFree { get; set; }

		[JsonIgnore]
		public virtual ICollection<User> Users { get; set; }
	}
}
