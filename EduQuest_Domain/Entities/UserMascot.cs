using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	public partial class UserMascot: BaseEntity
	{
        public string UserId { get; set; }
        public string ShopItemId { get; set; }

		[JsonIgnore]
		public virtual ICollection<ShopItem> ShopItems { get; set; } 

		[JsonIgnore]
		public virtual User User { get; set; } = null!;
	}
}
