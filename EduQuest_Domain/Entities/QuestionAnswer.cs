using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("QuestionAnswer")]
	public partial class QuestionAnswer : BaseEntity
	{
		public string Title { get; set; } = string.Empty;
		public bool IsCorrect { get; set; }
		public string QuestionId { get; set; }

		public virtual Question Question { get; set; } = null!;
	}
}
