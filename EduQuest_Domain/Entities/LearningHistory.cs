using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("LearningHistory")]
	public partial class LearningHistory : BaseEntity
	{
		public Guid UserId { get; set; }
		public Guid CourseId { get; set; }
		public DateTime LastAccessed { get; set; }

		public virtual User User { get; set; } = null;
		public virtual Course Course { get; set; } = null;
	}
}
