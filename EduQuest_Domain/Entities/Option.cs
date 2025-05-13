using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("Option")]
	public partial class Option : BaseEntity
	{
		public string QuestionId { get; set; }
		public string AnswerContent { get; set; }
		public bool IsCorrect { get; set; }

		public virtual Question Question { get; set; }
	}
}
