using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Mascots.Commands.EquipMacotItem;

public class EquipMascotItemCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public List<string> ItemIds { get; set; }
}

