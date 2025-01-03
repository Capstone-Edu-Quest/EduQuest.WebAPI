using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Question")]
	public partial class Question : BaseEntity
	{
		public string QuizId { get; set; }
		public string QuestionTitle { get; set; }
		public bool MultipleAnswers { get; set; }

		public virtual Quiz Quiz { get; set; } = null!;
		public virtual ICollection<Answer> Answers { get; set; }
	}
}
