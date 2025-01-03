using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("Answer")]
	public partial class Answer : BaseEntity
	{
		public string QuestionId { get; set; }
		public string AnswerContent { get; set; }
		public bool IsCorrect { get; set; }

		public virtual Question Question { get; set; }
	}
}
