using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Cart")]
	public partial class Cart : BaseEntity
	{
		public string UserId { get; set; }
		//public string CourseId { get; set; }
		public decimal TotalPrice { get; set; }
		public string Status { get; set; } = null!;

		[JsonIgnore]
		public virtual User User { get; set; } = null!;
		[JsonIgnore]
		public virtual Course Course { get; set; } = null!;

		[JsonIgnore]
		public virtual ICollection<Payment> Payments { get; set; }
		[JsonIgnore]
		public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

		
	}
}
