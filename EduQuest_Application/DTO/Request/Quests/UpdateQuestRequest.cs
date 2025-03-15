using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using static EduQuest_Domain.Constants.Constants;
using System.ComponentModel.DataAnnotations;

namespace EduQuest_Application.DTO.Request.Quests
{
    public class UpdateQuestRequest : IMapFrom<Quest>, IMapTo<Quest>
    {
        public string? Title { get; set; } //change name to title

        public int? PointToComplete { get; set; }

        public bool? IsDaily { get; set; }

        public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định

        public string? Description { get; set; }

        public int? TimeToComplete { get; set; } //default value is minute

        public List<UpdateQuestRewardRequest> UpdatedRewards { get; set; }
    }
}
