using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Transaction")]
	public partial class Transaction : BaseEntity
	{
		public string UserId { get; set; }
		public decimal TotalAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? StripeFee { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string? PaymentIntentId { get; set; }
		public string? CustomerEmail { get; set; }
		public string? CustomerName { get; set; }
        public string? Url { get; set; }

        [JsonIgnore]
		public virtual User User { get; set; } = null;
		
		
	}
}
