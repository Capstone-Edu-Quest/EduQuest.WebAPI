using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Badge")]
	public partial class Badge : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string IconUrl { get; set; }
		public string Color { get; set; }
		public virtual ICollection<User> Users { get; set; }
	}
}
