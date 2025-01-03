using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("LearningMaterial")]
	public partial class LearningMaterial : BaseEntity
	{
		public string StageId { get; set; }
		public string Type { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string UrlMaterial { get; set; }

		public virtual Stage Stage { get; set; } = null;
	}
}
