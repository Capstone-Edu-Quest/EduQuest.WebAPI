using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("LearnerStatistic")]
	public partial class LearnerStatistic 
	{
		public Guid CourseId { get; set; }
		public Guid UserId { get; set; }
		public int ProgressPercentage { get; set; }
		public int Xp { get; set; }
		public int Gold { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }

		public DateTime? DeletedAt { get; set; }

		public virtual User User { get; set; } = null;
		public virtual Course Course { get; set; } = null;
	}
}
