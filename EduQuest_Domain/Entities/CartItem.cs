using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("CartItem")]
	public partial class CartItem : BaseEntity
	{
		public string CartId { get; set; }

		public string CourseId { get; set; }

		public decimal Price { get; set; }

		public virtual Cart? Cart { get; set; }

		public virtual Course? Course { get; set; }
	}
}
