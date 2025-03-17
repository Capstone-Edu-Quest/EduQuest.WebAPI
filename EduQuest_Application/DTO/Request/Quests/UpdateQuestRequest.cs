using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using static EduQuest_Domain.Constants.Constants;
using System.ComponentModel.DataAnnotations;

namespace EduQuest_Application.DTO.Request.Quests
{
    public class UpdateQuestRequest : IMapFrom<Quest>, IMapTo<Quest>
    {
        public string? Title { get; set; } //change name to title
        public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định
        public int? QuestType { get; set; }// learning streak, complete courses,....
        public int[] QuestValue { get; set; }

        public int[] RewardType { get; set; }
        public int[] RewardValue { get; set; }

    }
}
