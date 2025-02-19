using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("UserStatistic")]
	public partial class UserStatistic : BaseEntity
	{
		public string UserId { get; set; }
		public int? CurrentStreak { get; set; }
		public int? LongestStreak { get; set; }
		public DateTime? LastLearningDay { get; set; }
		public int? TotalCompletedCourses { get; set; }
        public int? Gold { get; set; }
        public int? Exp { get; set; }
        public int? Level { get; set; }
        public int? TotalStudyTime { get; set; }
        public int? TotalCourseCreated { get; set; }
		public int? TotalLearner { get; set; }
		public int? TotalReview { get; set; }

		public virtual User User { get; set; } = null!;
        public virtual ICollection<StudyTime>? StudyTime { get; set; } = null!;
    }
}
