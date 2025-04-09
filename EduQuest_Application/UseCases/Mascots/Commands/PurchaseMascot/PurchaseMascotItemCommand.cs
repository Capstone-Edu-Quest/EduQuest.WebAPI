using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Mascots.Commands.PurchaseMascot;

public class PurchaseMascotItemCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string Name { get; set; }
}
