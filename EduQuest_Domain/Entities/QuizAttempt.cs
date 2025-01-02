using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("QuizAttempt")]
	public partial class QuizAttempt : BaseEntity
	{
		public string QuizId { get; set; }
		public string UserId { get; set; }
		public int CorrectAnswers { get; set; }
		public int IncorrectAnswers { get; set; }

		public Quiz Quiz { get; set; } = null!;
		public User User { get; set; } = null!;
	}
}
