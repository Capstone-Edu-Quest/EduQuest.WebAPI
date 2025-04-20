using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("CourseLearner")]
	public class CourseLearner : BaseEntity
	{
		public string UserId { get; set; }
		public string CourseId { get; set; }
        public bool IsActive { get; set; }
        public string? CurrentLessonId { get; set; } 
        public string? CurrentMaterialId { get; set; }
        public double? TotalTime { get; set; }
        public decimal? ProgressPercentage { get; set; }

		[JsonIgnore]
		public virtual User Users { get; set; } = null!;
		[JsonIgnore]
		public virtual Course Courses { get; set; } = null!;

		/*[JsonIgnore]
		public virtual ICollection<Feedback>? Feedbacks { get; set; }*/
	}
}
