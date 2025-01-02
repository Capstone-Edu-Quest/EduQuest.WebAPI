using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("PackagePrivilege")]
	public partial class PackagePrivilege : BaseEntity
	{
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }

		public virtual ICollection<AccountPackage> AccountPackages { get; set; }
	}
}
