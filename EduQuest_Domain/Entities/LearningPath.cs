using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("LearningPath")]
	public partial class LearningPath: BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string UserId { get; set; }

		public virtual User User { get; set; }
		public virtual ICollection<LearningPathCourse> LearningPathCourses { get; set; }
	}
}
