using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("CourseAchievement")]
	public partial class CourseAchievement : BaseEntity
	{
		public string CourseId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public virtual Course Course { get; set; } = null;
	}
}
