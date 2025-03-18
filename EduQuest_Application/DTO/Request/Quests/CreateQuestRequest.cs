using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.DTO.Request.Quests
{
    public class CreateQuestRequest : IMapFrom<Quest>, IMapTo<Quest>
    {
        [Required(ErrorMessage = MessageError.ValueRequired)]
        public string? Title { get; set; } //change name to title

        [Required(ErrorMessage = MessageError.ValueRequired)]
        public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định

        [Required(ErrorMessage = MessageError.ValueRequired)]
        public int? QuestType { get; set; }// learning streak, complete courses,....

        [Required(ErrorMessage = MessageError.ValueRequired)]
        public int[] QuestValue { get; set; }


        [Required(ErrorMessage = MessageError.ValueRequired)]
        public int[] RewardType { get; set; } 
        [Required(ErrorMessage = MessageError.ValueRequired)]
        public int[] RewardValue { get; set; }

        /*[Required(ErrorMessage = MessageError.ValueRequired)]
        public List<QuestRewardRequest> Rewards { get; set; }*/
    }
}
