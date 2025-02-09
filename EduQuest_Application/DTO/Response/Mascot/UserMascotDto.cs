using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Mascot;

public class UserMascotDto : IMapFrom<MascotInventory>, IMapTo<MascotInventory>
{ 
    public string ShopItemId { get; set; }
    public bool IsEquipped { get; set; }
}
