using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
