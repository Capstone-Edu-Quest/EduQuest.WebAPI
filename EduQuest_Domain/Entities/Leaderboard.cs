using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("Leaderboard")]
	public partial class Leaderboard : BaseEntity
	{
		public string UserId { get; set; }
		public string TotalTime { get; set; }
		public int NumOfCourse { get; set; }

		public virtual ICollection<User> Users { get; set; } = null;
		
	}
}
