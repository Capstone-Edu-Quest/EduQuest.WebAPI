using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Subscription")]
	public partial class Subscription : BaseEntity
	{
		
        public string RoleId { get; set; }
		public string PackageType { get; set; }
        public string Config { get; set; }
        public decimal Value { get; set; }

        [JsonIgnore]
		public virtual Role? Role { get; set; } = null!;
		[JsonIgnore]
		public virtual ICollection<User?> Users { get; set; }
	}
}
