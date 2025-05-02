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
        public virtual ICollection<User> Users { get; set; } = new List<User>();


        [JsonIgnore]
		public virtual ICollection<Course> Courses { get; set; }

        [JsonIgnore]
        public virtual ICollection<LearningPath> LearningPaths { get; set; }
    }
}
