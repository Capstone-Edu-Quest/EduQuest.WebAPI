using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Tag")]
	public partial class Tag: BaseEntity
	{
		public string Name { get; set; }

		public virtual ICollection<Course> Courses { get; set; }
	}
}
