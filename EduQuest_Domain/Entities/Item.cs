using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Item")]
	public partial class Item : BaseEntity
	{
		public string Name { get; set; }
		public int Price { get; set; }
		public string Description { get; set; }

		public virtual ICollection<Course> Courses { get; set; }
	}
}
