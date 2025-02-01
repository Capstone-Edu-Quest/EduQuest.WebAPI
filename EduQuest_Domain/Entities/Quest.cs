using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Quest")]
	public partial class Quest : BaseEntity
	{
		public string? Name { get; set; }
        public string? Type { get; set; }
		public string? Description { get; set; }
		public string? RewardType { get; set; }
		public string? RewardValue { get; set; }
		
		public string? Image { get; set; }
		public string? Color { get; set; }
        public string? Condition { get; set; }

        [JsonIgnore]
		public virtual ICollection<User> Users { get; set; }
	}
}
