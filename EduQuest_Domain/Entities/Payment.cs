using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Payment")]
	public partial class Payment : BaseEntity
	{
		public string PaymentMethod { get; set; }
		public decimal PaidAmount { get; set; }
		public DateTime PaidDate { get; set; }
		public string CartId { get; set; }
		public decimal TotalAmount { get; set; }

		public virtual Cart Cart { get; set; } = null;
		public virtual ICollection<Transaction> Transactions { get; set; }
	}
}
