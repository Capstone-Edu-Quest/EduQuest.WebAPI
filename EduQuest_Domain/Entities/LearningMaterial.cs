using System.ComponentModel.DataAnnotations.Schema;

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
        public int? Duration { get; set; }

        public virtual Stage Stage { get; set; } = null;
	}
}
