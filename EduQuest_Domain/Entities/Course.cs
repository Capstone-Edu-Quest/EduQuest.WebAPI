using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Course")]
	public partial class Course : BaseEntity
	{
		public string Title { get; set; } = string.Empty;
		public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Color { get; set; }
		public decimal? Price { get; set; }
        public string? Requirement { get; set; }
        public bool IsRequired { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? LastUpdated { get; set; }

		[JsonIgnore]
		public virtual User User { get; set; } = null!;

		[JsonIgnore]
		public virtual ICollection<Certificate>? Certificates { get; set; }
		[JsonIgnore]
		public virtual ICollection<Tag>? Tags { get; set; }
		[JsonIgnore]
		public virtual ICollection<Item>? Items { get; set; }
		[JsonIgnore]
		public virtual ICollection<Cart>? Carts { get; set; }
		[JsonIgnore]
		public virtual ICollection<FavoriteList>? FavoriteLists { get; set; }
		[JsonIgnore]
		public virtual ICollection<Stage>? Stages { get; set; }
	}
}
