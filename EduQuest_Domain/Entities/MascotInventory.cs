using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
    public partial class MascotInventory: BaseEntity
	{
        public string UserId { get; set; }
        public string ShopItemId { get; set; }
        public bool IsEquipped { get; set; }

        [JsonIgnore]
		public virtual ICollection<ShopItem> ShopItems { get; set; } 

		[JsonIgnore]
		public virtual User User { get; set; } = null!;
	}
}
