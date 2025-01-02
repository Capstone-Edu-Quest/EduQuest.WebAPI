using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Transaction")]
	public partial class Transaction : BaseEntity
	{
		public string UserId { get; set; }

		public decimal TotalAmount { get; set; }

		public virtual User User { get; set; } = null;
		public virtual ICollection<Payment> Payments { get; set; } = null;
	}
}
