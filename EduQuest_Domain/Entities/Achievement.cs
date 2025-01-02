using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Achievement")]
	public partial class Achievement : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public string Color { get; set; }
		public virtual ICollection<User> Users { get; set; }
	}
}
