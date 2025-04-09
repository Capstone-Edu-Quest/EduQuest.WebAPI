using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("QuizAttempt")]
	public partial class QuizAttempt : BaseEntity
	{
		public string QuizId { get; set; }
		public string UserId { get; set; }
		public string LessonId { get; set; }
		public int CorrectAnswers { get; set; }
		public int IncorrectAnswers { get; set; }
		public double Percentage { get; set; }
        public int AttemptNo { get; set; }
		public int TotalTime { get; set; }
        public DateTime? SubmitAt { get; set; }

		[JsonIgnore]
        public virtual Quiz Quiz { get; set; } = null!;
        [JsonIgnore]
        public virtual User User { get; set; } = null!;

		public virtual ICollection<UserQuizAnswers> Answers { get; set; }
	}
}
