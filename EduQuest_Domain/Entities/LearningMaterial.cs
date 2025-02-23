using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("LearningMaterial")]
	public partial class LearningMaterial : BaseEntity
	{
		public string StageId { get; set; }
		public string Type { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int? Duration { get; set; }

		//Video
		public string UrlMaterial { get; set; }
        public string? Thumbnail { get; set; }
		//Document
        public string? Content { get; set; }


		[JsonIgnore]
		public virtual ICollection<Stage> Stages { get; set; }
		[JsonIgnore]
		public virtual Quiz? Quiz { get; set; } = null;
		[JsonIgnore]
		public virtual Assignment? Assignment { get; set; } = null;
	}
}
