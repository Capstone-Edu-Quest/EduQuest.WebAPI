using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Mascot.Commands;

public class PurchaseMascotItemCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string ShopItemId { get; set; }
}
