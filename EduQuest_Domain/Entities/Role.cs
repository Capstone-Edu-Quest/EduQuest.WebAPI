using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Role")]
	public partial class Role : BaseEntity
	{
		public string RoleName { get; set; } = null!;

		[JsonIgnore]
		public virtual ICollection<User>? Users { get; set; }
	}
}
