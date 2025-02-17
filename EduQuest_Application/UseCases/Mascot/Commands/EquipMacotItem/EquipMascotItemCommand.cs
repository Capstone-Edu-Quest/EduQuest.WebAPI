using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Mascot.Commands.EquipMacotItem;

public class EquipMascotItemCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string ShopItemId { get; set; }
}

