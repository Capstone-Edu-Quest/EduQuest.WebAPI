using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("UserMeta")]
	public partial class UserMeta : BaseEntity
	{
		public string UserId { get; set; }
		public int? CurrentStreak { get; set; }
		public int? LongestStreak { get; set; }
		public DateTime? LastLearningDay { get; set; }
		public int? TotalCompletedCourses { get; set; }
        public int? Gold { get; set; }
        public int? Exp { get; set; }
        public int? Level { get; set; }
        public double? TotalStudyTime { get; set; }
        public int? TotalCourseCreated { get; set; }
		public int? TotalLearner { get; set; }
		public int? TotalReview { get; set; }
        public double? TotalRevenue { get; set; }
		public DateTime LastActive { get; set; }
        public decimal? HeldAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        [JsonIgnore]
		public virtual User User { get; set; } = null!;
		[JsonIgnore]
		public virtual ICollection<StudyTime>? StudyTime { get; set; } = null!;
    }
}
