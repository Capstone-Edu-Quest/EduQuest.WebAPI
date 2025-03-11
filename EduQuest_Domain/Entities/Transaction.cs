using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Transaction")]
	public partial class Transaction : BaseEntity
	{
		public string UserId { get; set; }

		public decimal TotalAmount { get; set; }
		public string Status { get; set; }
        public string Type { get; set; }
        public string PaymentIntentId { get; set; }

		[JsonIgnore]
		public virtual User User { get; set; } = null;
		
		
	}
}
