using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Course")]
	public partial class Course : BaseEntity
	{
		public string Title { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? Color { get; set; }
		public decimal Price { get; set; }
		public bool IsRequired { get; set; }
		public Guid CreatedBy { get; set; }

		public virtual ICollection<Certificate> Certificates { get; set; }
	}
}
