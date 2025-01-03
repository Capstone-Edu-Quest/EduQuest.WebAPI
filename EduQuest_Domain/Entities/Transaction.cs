using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Transaction")]
	public partial class Transaction : BaseEntity
	{
		public string UserId { get; set; }

		public decimal TotalAmount { get; set; }

		public virtual User User { get; set; } = null;
		[JsonIgnore]
		public virtual ICollection<Payment> Payments { get; set; } = null;
	}
}
