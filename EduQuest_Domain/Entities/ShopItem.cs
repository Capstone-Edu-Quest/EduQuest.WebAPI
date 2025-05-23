﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities
{
    [Table("ShopItem")]
	public partial class ShopItem : BaseEntity
	{
        public string Name { get; set; }
        public double Price { get; set; }
        public string? TagId { get; set; }
        [JsonIgnore]
        public virtual ICollection<Mascot>? MascotItems { get; set; }
        [JsonIgnore]
        public virtual Tag Tag { get; set; }
    }

}

