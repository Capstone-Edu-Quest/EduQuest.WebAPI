using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Badges.Queries;

public class GetBadgesQuery : IRequest<APIResponse>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public string? Color { get; set; }
}
