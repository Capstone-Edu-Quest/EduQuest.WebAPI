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
		public decimal? Price { get; set; }
        public string? Requirement { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string? AssignTo { get; set; }
        public string? RejectedReason { get; set; }
		public int? Version { get; set; }
		public string? OriginalCourseId { get; set; }

		//public DateTime? LastUpdated { get; set; }

		[JsonIgnore]
		public virtual User User { get; set; } = null!;
		[JsonIgnore]
		public virtual CourseStatistic CourseStatistic { get; set; } = null!;

		[JsonIgnore]
		public virtual ICollection<Certificate>? Certificates { get; set; }
		[JsonIgnore]
		public virtual ICollection<CourseLearner>? CourseLearners { get; set; }
		[JsonIgnore]
		public virtual ICollection<Feedback>? Feedbacks { get; set; }
		[JsonIgnore]
		public virtual ICollection<Tag>? Tags { get; set; }
		
		[JsonIgnore]
		public virtual ICollection<Cart>? Carts { get; set; }
		[JsonIgnore]
		public virtual ICollection<FavoriteList>? FavoriteLists { get; set; } = new List<FavoriteList>();
		[JsonIgnore]
		public virtual ICollection<Lesson>? Lessons { get; set; }

        /*[JsonIgnore]
        public virtual ICollection<Coupon>? Coupons { get; set; }*/
		[JsonIgnore]
		public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        [JsonIgnore]
        public virtual ICollection<Report> Reports { get; set; }
    }
}
