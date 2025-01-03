using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Reward")]
	public partial class Reward : BaseEntity
	{
		public string StageId { get; set; }
		public string RewardType { get; set; }
		public int RewardValue { get; set; }

		public virtual Stage Stage { get; set; } = null;
	}
}
