using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Material")]
	public partial class Material : BaseEntity
	{
		//public string StageId { get; set; }
		public string Type { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
        public int? Version { get; set; }
        public string? OriginalMaterialId { get; set; }
        public double? Duration { get; set; }
        public string UserId { get; set; }
        
        //Video
        public string? UrlMaterial { get; set; }
        public string? Thumbnail { get; set; }
		//Document
        public string? Content { get; set; }
        

		[JsonIgnore]
		public virtual ICollection<LessonMaterial> LessonMaterials { get; set; }
		
		[JsonIgnore]
		public virtual User? User { get; set; } = null;

	}
}
