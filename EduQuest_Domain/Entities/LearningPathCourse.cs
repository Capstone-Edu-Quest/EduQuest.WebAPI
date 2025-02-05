using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("LearningPathCourse")]
	public partial class LearningPathCourse
	{
		public string CourseId { get; set; }
		public string LearningPathId { get; set; }
		public int CourseOrder { get; set; }
        public DateTime CompletedAt { get; set; }



        public virtual LearningPath LearningPath { get; set; } = null;
		public virtual Course Course { get; set; } = null;
	}
}
