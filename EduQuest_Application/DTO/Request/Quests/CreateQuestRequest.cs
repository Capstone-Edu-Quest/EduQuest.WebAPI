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
        public int? PointToComplete { get; set; }

        [Required(ErrorMessage = MessageError.ValueRequired)]
        public bool? IsDaily { get; set; }

        [Required(ErrorMessage = MessageError.ValueRequired)]
        public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định

        [Required(ErrorMessage = MessageError.ValueRequired)]
        public string? Description { get; set; }

        [Required(ErrorMessage = MessageError.ValueRequired)]
        public int? TimeToComplete { get; set; } //default value is minute

        [Required(ErrorMessage = MessageError.ValueRequired)]
        public List<QuestRewardRequest> Rewards { get; set; }
    }
}
