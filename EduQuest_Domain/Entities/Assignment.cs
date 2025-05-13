using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Assignment")]
	public class Assignment : BaseEntity
	{
		public string UserId { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public double? TimeLimit { get; set; }
		public string? Question { get; set; }
		public string? AnswerLanguage { get; set; }
		public string? ExpectedAnswer { get; set; }

		[JsonIgnore]
		public virtual ICollection<LessonContent>? LessonMaterials { get; set; }
		[JsonIgnore]
		public virtual ICollection<Tag>? Tags { get; set; }
		[JsonIgnore]
		public virtual User? User { get; set; } = null;
	}
}
