using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
