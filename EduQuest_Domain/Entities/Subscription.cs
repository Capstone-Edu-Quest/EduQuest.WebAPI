using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("Subscription")]
	public partial class Subscription : BaseEntity
	{
		public string Package { get; set; }  
		public string? Type { get; set; } 
		public decimal? MonthlyPrice { get; set; } 
		public decimal? YearlyPrice { get; set; }
		public decimal? Value { get; set; }
		public string? BenefitsJson { get; set; }

		[JsonIgnore]
		public virtual ICollection<User> Users { get; set; }
	}
}
