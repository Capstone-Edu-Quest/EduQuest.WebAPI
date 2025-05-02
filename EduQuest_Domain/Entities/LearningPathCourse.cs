using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("LearningPathCourse")]
	public partial class LearningPathCourse : BaseEntity
	{
		public string CourseId { get; set; }
		public string LearningPathId { get; set; }
		public int CourseOrder { get; set; }
        public virtual LearningPath LearningPath { get; set; } = null;
		public virtual Course Course { get; set; } = null;
	}
}
