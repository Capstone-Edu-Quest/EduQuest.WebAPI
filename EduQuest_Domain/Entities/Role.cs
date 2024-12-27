using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	public class Role
	{
		public int RoleId { get; set; }
		public string RoleName { get; set; } = null!;

		[JsonIgnore]
		public virtual ICollection<User>? Users { get; set; }
	}
}
