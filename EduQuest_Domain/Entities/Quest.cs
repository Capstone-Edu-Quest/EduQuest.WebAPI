using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Quest")]
	public partial class Quest : BaseEntity
	{
		public string? Title { get; set; } //change name to title
        public bool? IsDaily { get; set; }
        public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định
		public string? Description { get; set; }
		public int? PointToComplete { get; set; }
        public string? TimeToComplete { get; set; }
        public string? CreatedBy { get; set; }

        [JsonIgnore]
		public virtual User User { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestReward> Rewards { get; set; }

        /*MissionType
          RewardType
          Type
          là dev tạo*/

    }
}
