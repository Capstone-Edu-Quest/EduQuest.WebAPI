using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	public partial class Level : BaseEntity
	{
        public string Name { get; set; }
        public string UpLevelReward { get; set; }
        public string ExpPerLevel { get; set; }
        public string LevelRequirement { get; set; }
    }
}
