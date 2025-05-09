using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Quiz")]
	public partial class Quiz : BaseEntity
	{

		public double? TimeLimit { get; set; }
		public decimal PassingPercentage { get; set; }

		[JsonIgnore]
		public virtual ICollection<Question> Questions { get; set; }
	}
}
