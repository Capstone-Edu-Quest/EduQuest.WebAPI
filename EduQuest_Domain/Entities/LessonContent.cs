using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("LessonContent")]
	public class LessonContent : BaseEntity
	{
        public string LessonId { get; set; }
        public string? MaterialId { get; set; }
		public string? AssignmentId { get; set; }
		public string? QuizId { get; set; }
        public int Index { get; set; }

		//[JsonIgnore]
		public virtual Lesson Lesson { get; set; } = null;
		//[JsonIgnore]
		public virtual Material Material { get; set; } = null;
		public virtual Assignment Assignment { get; set; } = null;
		public virtual Quiz Quiz { get; set; } = null;
	}
}
