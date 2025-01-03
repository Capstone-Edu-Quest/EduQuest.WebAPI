using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("PackagePrivilege")]
	public partial class PackagePrivilege : BaseEntity
	{
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }

		[JsonIgnore]
		public virtual ICollection<AccountPackage> AccountPackages { get; set; }
	}
}
