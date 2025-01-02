using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("AccountPackage")]
	public partial class AccountPackage : BaseEntity
	{
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public int DurationDays { get; set; }
		public decimal Price { get; set; }
		public bool IsFree { get; set; }

		public virtual ICollection<PackagePrivilege> PackagePrivileges { get; set; }
	}
}
