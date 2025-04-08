using EduQuest_Application.Mappings;

namespace EduQuest_Application.DTO.Response.Mascot;

public class UserMascotDto : IMapFrom<EduQuest_Domain.Entities.Mascot>, IMapTo<EduQuest_Domain.Entities.Mascot>
{ 
    public string ShopItemId { get; set; }
    public bool IsEquipped { get; set; }
}
