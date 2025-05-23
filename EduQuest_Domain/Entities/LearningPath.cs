﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("LearningPath")]
	public partial class LearningPath : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string UserId { get; set; }
        public double? TotalTimes { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
		public bool IsPublic { get; set; }
		public bool IsEnrolled { get; set; }
		public bool CreatedByExpert { get; set; }
		

        [JsonIgnore]
        public virtual User User { get; set; }
		[JsonIgnore]
		public virtual ICollection<LearningPathCourse> LearningPathCourses { get; set; }

		[JsonIgnore]
		public virtual ICollection<Tag> Tags { get; set; }
		[JsonIgnore]
		public virtual ICollection<Enroller> Enrollers { get; set; }
	}
	
}
