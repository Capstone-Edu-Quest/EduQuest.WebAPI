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
		public string Title { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string QuizId { get; set; }

		public virtual Quiz Quiz { get; set; } = null!;
		public virtual ICollection<QuestionAnswer> Answers { get; set; }
	}
}
