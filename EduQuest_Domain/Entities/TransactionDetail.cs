using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("TransactionDetail")]
	public partial class TransactionDetail : BaseEntity
	{
        public string TransactionId { get; set; }
        public string InstructorId { get; set; }
        public string ItemType { get; set; }
        public string ItemId { get; set; }
        public decimal Amount { get; set; }
        public decimal? StripeFee { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? SystemShare { get; set; }
        public decimal? InstructorShare { get; set; }

		[JsonIgnore]
		public virtual Transaction Transaction { get; set; } = null;
	}
}
