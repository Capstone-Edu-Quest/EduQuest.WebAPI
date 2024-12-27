using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{

	[Table("Users")]
	public class User : BaseEntity
	{
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string Status { get; set; } = null!;
		public int RoleId { get; set; }
		public string? Avatar { get; set; }
		public bool IsPremiumUser { get; set; } = false;

		public virtual Role Role { get; set; } = null!;
	}
}
