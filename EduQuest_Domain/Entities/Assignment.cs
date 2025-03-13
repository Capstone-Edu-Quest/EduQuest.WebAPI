using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Assignment")]
	public class Assignment : BaseEntity
	{
		public int? TimeLimit { get; set; }
		public string? Question { get; set; }
		public string? AnswerLanguage { get; set; }
		public string? ExpectedAnswer { get; set; }

		[JsonIgnore]
		public virtual User User { get; set; } = null!;

		[JsonIgnore]
		public virtual Material Material { get; set; } = null!;
	}
}
