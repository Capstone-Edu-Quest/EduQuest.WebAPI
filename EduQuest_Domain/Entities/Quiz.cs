using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Quiz")]
	public partial class Quiz : BaseEntity
	{
		public string StageId { get; set; }
		public string QuizData { get; set; }
		public string CreatedBy { get; set; }

		public virtual User Creator { get; set; } = null!;
		public virtual Stage Stage { get; set; } = null!;
		public virtual ICollection<Question> Questions { get; set; }
	}
}
