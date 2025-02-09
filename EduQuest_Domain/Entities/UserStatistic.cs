using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("UserStatistic")]
	public partial class UserStatistic : BaseEntity
	{
		public string UserId { get; set; }
		public int? TotalActiveDay { get; set; }
		public int? MaxStudyStreakDay { get; set; }
		public DateTime? LastLearningDay { get; set; }
		public int? CompletedCourses { get; set; }
        public int? Gold { get; set; }
        public int? Exp { get; set; }
        public int? Level { get; set; }
        public int? StudyTime { get; set; }
        public int? TotalCourseCreated { get; set; }
		public int? TotalLearner { get; set; }
		public int? TotalReview { get; set; }

		public virtual User User { get; set; } = null!;
	}
}
