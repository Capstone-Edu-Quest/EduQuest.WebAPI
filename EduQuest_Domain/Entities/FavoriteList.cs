using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
	[Table("FavoriteList")]
	public partial class FavoriteList : BaseEntity
	{
		public string UserId { get; set; }

		[JsonIgnore]
		public virtual User User { get; set; } = null;
		[JsonIgnore]
		public virtual ICollection<Course>? Courses { get; set; }
		
	}
}
