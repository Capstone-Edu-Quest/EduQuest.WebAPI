using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{

	[Table("User")]
	public partial class User : BaseEntity
	{
		public string Username { get; set; } = string.Empty;
		public string? AvatarUrl { get; set; }
		public string Email { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public string? PasswordHash { get; set; }
		public string? PasswordSalt { get; set; }

		public int RoleId { get; set; }
		public int? PackagePrivilegeId { get; set; }
		public int? AccountPackageId { get; set; }

		public virtual Role Role { get; set; } = null!;
		public virtual ICollection<SearchHistory> SearchHistories { get; set; }
		public virtual ICollection<Achievement> Achievements { get; set; }
		public virtual ICollection<Badge> Badges { get; set; }
		public virtual ICollection<Course> CreatedCourses { get; set; }
		public virtual ICollection<Certificate> Certificates { get; set; }
		public virtual ICollection<Course> Courses { get; set; }
		public virtual ICollection<Cart> Carts { get; set; }
	}
}
