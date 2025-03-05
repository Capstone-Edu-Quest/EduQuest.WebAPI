using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Feedback")]
	public partial class Feedback : BaseEntity
	{
		public string UserId { get; set; }
		public string CourseId { get; set; }
		public int Rating { get; set; }
		public string Comment { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; } = null;
        [JsonIgnore]
        public virtual Course Course { get; set; } = null;

        [JsonIgnore]
        public virtual ICollection<Report> Reports { get; set; }
    }
}
