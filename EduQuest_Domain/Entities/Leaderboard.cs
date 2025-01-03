using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("Leaderboard")]
	public partial class Leaderboard : BaseEntity
	{
		public string UserId { get; set; }
		public string CourseId { get; set; }
		public int Rank { get; set; }
		public int Score { get; set; }

		public virtual User User { get; set; } = null;
		public virtual Course Course { get; set; } = null;
	}
}
