using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("QuizAttempt")]
	public partial class QuizAttempt : BaseEntity
	{
		public string QuizId { get; set; }
		public string UserId { get; set; }
		public int CorrectAnswers { get; set; }
		public int IncorrectAnswers { get; set; }

		public virtual Quiz Quiz { get; set; } = null!;
		public virtual User User { get; set; } = null!;
	}
}
