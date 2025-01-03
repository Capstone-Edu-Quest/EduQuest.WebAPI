using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Question")]
	public partial class Question : BaseEntity
	{
		public string QuizId { get; set; }
		public string QuestionTitle { get; set; }
		public bool MultipleAnswers { get; set; }

		public virtual Quiz Quiz { get; set; } = null!;
		[JsonIgnore]
		public virtual ICollection<Answer> Answers { get; set; }
	}
}
