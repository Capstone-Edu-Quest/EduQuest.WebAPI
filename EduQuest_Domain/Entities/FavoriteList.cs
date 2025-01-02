using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("FavoriteList")]
	public partial class FavoriteList
	{
		public string UserId { get; set; }
		public string CourseId { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }

		public virtual User User { get; set; } = null;
		public virtual Course Course { get; set; } = null;
	}
}
