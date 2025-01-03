using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Cart")]
	public partial class Cart : BaseEntity
	{
		public string UserId { get; set; }
		public string CourseId { get; set; }
		public decimal TotalPrice { get; set; }

		public virtual User User { get; set; } = null!;
		public virtual Course Course { get; set; } = null!;
		[JsonIgnore]
		public virtual ICollection<Payment> Payments { get; set; }
	}
}
