using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("Advertise")]
	public partial class Advertise : BaseEntity
	{
		
		public DateTime StartDate { get; set; }

		
		public DateTime EndDate { get; set; }

		public int Priority { get; set; } = 0;

		public string Title { get; set; }

		public string Description { get; set; }

		public string ImageUrl { get; set; }

		
		public string RedirectUrl { get; set; }

		public byte Status { get; set; } = 1; // 0: Ẩn, 1: Hiển thị, 2: Hết hạn

		public int Clicks { get; set; } = 0;

		[JsonIgnore]
		public virtual ICollection<Course> Courses { get; set; }
	}
}
