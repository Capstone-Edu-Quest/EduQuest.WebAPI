using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("LearningPathCourse")]
	public partial class LearningPathCourse
	{
		public string CourseId { get; set; }
		public string LearningPathId { get; set; }
		public DateTime CompletedAt { get; set; }
		public int CourseOrder { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }

		// Navigation properties
		public virtual LearningPath LearningPath { get; set; } = null;
		public virtual Course Course { get; set; } = null;
	}
}
