using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Stages
{
    public class CreateStageRequest : IMapFrom<Stage>, IMapTo<Stage>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public List<string> MaterialIds { get; set; }
    }
}
