using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Quests
{
    public class CreateQuestRequest : IMapFrom<Quest>, IMapTo<Quest>
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? RewardType { get; set; }
        public string? RewardValue { get; set; }

        public string? Image { get; set; }
        public string? Color { get; set; }
        public string? Condition { get; set; }
        public List<string> ListBadgeId { get; set; }
    }
}
