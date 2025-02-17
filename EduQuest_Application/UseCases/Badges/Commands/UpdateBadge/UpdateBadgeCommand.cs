using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Badges.Commands.UpdateBadge
{
    public record UpdateBadgeCommand(
        string Id,
        string Name,
        string Description,
        string IconUrl,
        string Color
    ) : IRequest<APIResponse>;
}
