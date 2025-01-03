using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("Reward")]
	public partial class Reward : BaseEntity
	{
		public string StageId { get; set; }
		public string RewardType { get; set; }
		public int RewardValue { get; set; }

		public virtual Stage Stage { get; set; } = null;
	}
}
