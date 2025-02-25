using EduQuest_Domain.Models.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace EduQuest_Application.UseCases.Badges.Commands.UpdateBadge;

public class UpdateBadgeCommand : IRequest<APIResponse>
{
    public string Id {  get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconUrl { get; set; }
    public string Color { get; set; }
    [JsonConstructor]
    public UpdateBadgeCommand(string badgeId, string name, string description, string iconUrl, string color)
    {
        Id = badgeId;
        Name = name;
        Description = description;
        IconUrl = iconUrl;
        Color = color;
    }
}
