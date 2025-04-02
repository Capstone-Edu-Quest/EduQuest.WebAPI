using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Shop.Commands.UpdateShopItem;

public class UpdateShopItemCommand : IRequest<APIResponse>
{
    public string Name { get; set; }
    public double Price { get; set; }
}
