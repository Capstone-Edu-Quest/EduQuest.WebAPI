using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities
{
	[Table("SearchHistory")]
	public partial class SearchHistory : BaseEntity
	{
		public string Keyword { get; set; } = string.Empty;
		public string UserId { get; set; }

		public virtual User User { get; set; } = null!;
	}
}
