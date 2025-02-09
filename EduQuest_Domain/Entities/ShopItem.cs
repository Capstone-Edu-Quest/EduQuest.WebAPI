using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
    [Table("ShopItem")]
	public partial class ShopItem : BaseEntity
	{
        public string Name { get; set; }
        public double Price { get; set; }

		[JsonIgnore]
		public virtual MascotInventory? UserMascot { get; set; }

	}
}
