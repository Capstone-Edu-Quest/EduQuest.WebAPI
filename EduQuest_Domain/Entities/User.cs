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
		public string? Description { get; set; }
		public string? PasswordHash { get; set; }
		public string? PasswordSalt { get; set; }
		public string? RoleId { get; set; }
		public string? PackagePrivilegeId { get; set; }
		public string? AccountPackageId { get; set; }

		[JsonIgnore]
		public virtual Role? Role { get; set; } = null!;
		[JsonIgnore]
		public virtual AccountPackage? AccountPackage { get; set; } = null!;
		[JsonIgnore]
		public virtual PackagePrivilege? PackagePrivilege { get; set; } = null!;
		[JsonIgnore]
		public virtual UserStatistic? UserStatistic { get; set; } = null!;

		[JsonIgnore]
		public virtual ICollection<SearchHistory>? SearchHistories { get; set; }
		[JsonIgnore]
		public virtual ICollection<Achievement>? Achievements { get; set; }
		[JsonIgnore]
		public virtual ICollection<Badge>? Badges { get; set; }
		[JsonIgnore]
		public virtual ICollection<Certificate>? Certificates { get; set; }
		[JsonIgnore]
		public virtual ICollection<Course>? Courses { get; set; }
		[JsonIgnore]
		public virtual ICollection<Cart>? Carts { get; set; }
		[JsonIgnore]
		public virtual ICollection<FavoriteList>? FavoriteLists { get; set; }
		
	}
}
