using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static EduQuest_Domain.Enums.QuestEnum;

namespace EduQuest_Domain.Entities
{
	public partial class Level : BaseEntity
	{
        public int LevelNumber { get; set; } 
        public int Exp { get; set; }
        public string? RewardTypes { get; set; }
        public string? RewardValues { get; set; }
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();

    }

	
}
