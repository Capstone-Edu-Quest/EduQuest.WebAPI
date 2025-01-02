using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Certificate")]
	public partial class Certificate : BaseEntity
	{
		public string Title { get; set; } = string.Empty;
		public string? Url { get; set; }
		public string UserId { get; set; }
		public string CourseId { get; set; }

		public virtual User User { get; set; } = null!;
		public virtual Course Course { get; set; } = null!;
	}
}
