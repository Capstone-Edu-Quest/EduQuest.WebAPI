using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("LearningPathCourse")]
	public partial class LearningPathCourse : BaseEntity
	{
		public string CourseId { get; set; }
		public string LearningPathId { get; set; }
		public int CourseOrder { get; set; }
		public DateTime? DueDate { get; set; }
		public bool IsOverDue { get; set; } = false;
		public bool IsCompleted { get; set; } = false;
        public virtual LearningPath LearningPath { get; set; } = null;
		public virtual Course Course { get; set; } = null;
	}
}
