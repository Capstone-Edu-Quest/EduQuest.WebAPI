using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{

	[Table("Users")]
	public class User : BaseEntity
	{
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string Status { get; set; } = null!;
		public string RoleId { get; set; }
		public string? Avatar { get; set; }
		public bool IsPremiumUser { get; set; } = false;

		public virtual Role Role { get; set; } = null!;
	}
}
