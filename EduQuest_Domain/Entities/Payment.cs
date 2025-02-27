using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Payment")]
	public partial class Payment : BaseEntity
	{
		public string PaymentMethod { get; set; }
		public decimal PaidAmount { get; set; }
		//public DateTime PaidDate { get; set; }
		public string CartId { get; set; }
		public decimal TotalAmount { get; set; }

		public virtual Cart Cart { get; set; } = null;
		[JsonIgnore]
		public virtual ICollection<Transaction> Transactions { get; set; }
	}
}
