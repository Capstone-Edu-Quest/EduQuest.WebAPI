using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{

	[Table("User")]
	public partial class User : BaseEntity
	{
		public string? Username { get; set; } = string.Empty;
		public string? AvatarUrl { get; set; }
		public string? Email { get; set; } = string.Empty;
		public string? Phone { get; set; } = string.Empty;
		public string? Headline { get; set; }
		public string? PasswordHash { get; set; }
		public string? PasswordSalt { get; set; }
        public string Status { get; set; } = null!;
        public string? Description { get; set; }
        public string? AssignToExpertId { get; set; }
        public string? RejectedReason { get; set; }
        public string? RoleId { get; set; }
        public string? StripeAccountId { get; set; }
        public string? StripeAccountUrl { get; set; }
		public string Package { get; set; }
        public DateTime? PackageExperiedDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserTag> UserTags { get; set; } = new List<UserTag>();


        [JsonIgnore]
		public virtual Role? Role { get; set; } = null!;
		[JsonIgnore]
		public virtual FavoriteList? FavoriteList { get; set; } = null!;
		[JsonIgnore]
		public virtual Cart? Carts { get; set; } = null!;

		[JsonIgnore]
		public virtual Levels? Level { get; set; } = null!;


        [JsonIgnore]
		public virtual UserMeta? UserMeta { get; set; } = null!;
		[JsonIgnore]
		public virtual Subscription? Subscription { get; set; } = null!;

		[JsonIgnore]
		public virtual ICollection<Mascot>? MascotItem { get; set; } = null!;
        
       
		[JsonIgnore]
		public virtual ICollection<UserQuest>? UserQuests { get; set; }
		
		
		[JsonIgnore]
		public virtual ICollection<Certificate>? Certificates { get; set; }
		[JsonIgnore]
		public virtual ICollection<Course>? Courses { get; set; }
		
		
        [JsonIgnore]
        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }

		[JsonIgnore]
		public virtual ICollection<Coupon>? Coupons { get; set; }
		[JsonIgnore]
		public virtual ICollection<Transaction>? Transactions { get; set; }

		[JsonIgnore]
		public virtual ICollection<Quest> Quests { get; set; }

        [JsonIgnore]
        public virtual ICollection<Report> Reports { get; set; }
		[JsonIgnore]
		public virtual ICollection<LearningPath> LearningPaths { get; set; }
		[JsonIgnore]
		public virtual ICollection<Booster> Boosters { get; set; }

        [JsonIgnore]
        public virtual ICollection<InstructorCertificate> InstructorCertificates { get; set; } 

    }
}
