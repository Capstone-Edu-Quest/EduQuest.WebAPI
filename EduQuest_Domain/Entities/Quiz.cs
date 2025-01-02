using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Quiz")]
	public partial class Quiz : BaseEntity
	{
		public string Title { get; set; } = string.Empty;
		public string? Description { get; set; }
		public Guid CreatedBy { get; set; }

		public virtual User Creator { get; set; } = null!;
	}
}
