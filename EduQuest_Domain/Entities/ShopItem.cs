using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("ShopItem")]
	public partial class ShopItem : BaseEntity
	{
        public string Name { get; set; }
        public string Slot { get; set; }
        public double Price { get; set; }

		[JsonIgnore]
		public virtual ICollection<UserMascot> UserMascot { get; set; }

	}
}
