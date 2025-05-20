using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Tag")]
	public partial class Tag: BaseEntity
	{
		public string Name { get; set; }
        //public string? Level { get; set; }
        //public int? Grade { get; set; }
        public string? Type { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserTag> UserTags { get; set; } = new List<UserTag>();

        [JsonIgnore]
		public virtual ICollection<Course> Courses { get; set; }
		[JsonIgnore]
		public virtual ICollection<Material> Materials { get; set; }
		[JsonIgnore]
		public virtual ICollection<Quiz> Quizzes { get; set; }
		[JsonIgnore]
		public virtual ICollection<Assignment> Assignments { get; set; }

		[JsonIgnore]
        public virtual ICollection<LearningPath> LearningPaths { get; set; }
		[JsonIgnore]
		public virtual ICollection<ShopItem> ShopItems { get; set; }
    }
}
