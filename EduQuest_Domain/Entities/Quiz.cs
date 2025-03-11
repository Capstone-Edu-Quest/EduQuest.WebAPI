using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Quiz")]
	public partial class Quiz : BaseEntity
	{

		public int TimeLimit { get; set; }
		public decimal PassingPercentage { get; set; }
		public string CreatedBy { get; set; }

		[JsonIgnore]
		public virtual User User { get; set; } = null!;

		[JsonIgnore]
		public virtual Material Material { get; set; } = null!;

		[JsonIgnore]
		public virtual ICollection<Question> Questions { get; set; }
	}
}
