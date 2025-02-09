using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO;

public class ShopItemResponseDto : IMapFrom<ShopItem>, IMapTo<ShopItem>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public bool IsOwned { get; set; }
}
