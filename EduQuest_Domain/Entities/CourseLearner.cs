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
		public int ProgressPercentage { get; set; }

		public virtual ICollection<User> Users { get; set; } 
		public virtual ICollection<Course> Courses { get; set; } 

		/*[JsonIgnore]
		public virtual ICollection<Feedback>? Feedbacks { get; set; }*/
	}
}
