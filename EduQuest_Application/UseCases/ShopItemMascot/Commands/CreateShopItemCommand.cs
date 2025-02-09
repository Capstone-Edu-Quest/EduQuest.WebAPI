using EduQuest_Application.DTO.Request.ShopItem;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.ShopItemMascot.Commands;

public class CreateShopItemCommand : IRequest<APIResponse>
{
    public List<ShopItemDto> ShopItems { get; set; }
}
