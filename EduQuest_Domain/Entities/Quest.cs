using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Quest")]
	public partial class Quest : BaseEntity
	{
		public string? Title { get; set; } //change name to title
        public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định
        public int? QuestType { get; set; }// learning streak, complete courses,....
		public string? QuestValues { get; set; } //break to array when response to fe.
        public string? CreatedBy { get; set; }

        [JsonIgnore]
		public virtual User User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Reward> Rewards { get; set; }

        /*MissionType
          RewardType
          Type
          là dev tạo*/

    }
}
