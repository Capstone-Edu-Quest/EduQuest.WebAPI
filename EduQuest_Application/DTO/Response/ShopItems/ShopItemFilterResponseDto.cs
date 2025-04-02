using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.ShopItems;

public class ShopItemFilterResponseDto : IMapFrom<ShopItem>, IMapTo<ShopItem>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}
