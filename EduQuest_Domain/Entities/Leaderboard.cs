using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("Leaderboard")]
	public partial class Leaderboard : BaseEntity
	{
		public string UserId { get; set; }
		
		public string TotalTime { get; set; }
		public int NumOfCourse { get; set; }

		public virtual User User { get; set; } = null;
		
	}
}
