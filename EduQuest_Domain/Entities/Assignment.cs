using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Assignment")]
	public class Assignment : BaseEntity
	{
		public double? TimeLimit { get; set; }
		public string? Question { get; set; }
		public string? AnswerLanguage { get; set; }
		public string? ExpectedAnswer { get; set; }

		[JsonIgnore]
		public virtual Material Material { get; set; } = null!;
	}
}
