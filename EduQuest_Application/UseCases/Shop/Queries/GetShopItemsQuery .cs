using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.ShopItem.Queries;

public class GetShopItemsQuery : IRequest<APIResponse>
{
    public string UserId { get; set; }

    public GetShopItemsQuery(string userId)
    {
        UserId = userId;
    }
}
