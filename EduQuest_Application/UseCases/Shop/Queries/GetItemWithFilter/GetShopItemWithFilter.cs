using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Shop.Queries.GetItemWithFilterl;

public class GetShopItemWithFilter : IRequest<APIResponse>
{
    public string? Name { get; set; }
    public bool? IsGold { get; set; }
}
