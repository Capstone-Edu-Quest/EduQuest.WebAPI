using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Lesson")]
	public partial class Lesson : BaseEntity
	{
		public string CourseId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Index { get; set; }
        public double? TotalTime { get; set; }

		[JsonIgnore]
		public virtual Course Course { get; set; } = null;
		
		[JsonIgnore]
		public virtual ICollection<LessonContent> LessonMaterials { get; set; }
    }
}
