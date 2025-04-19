using EduQuest_Domain.Models.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace EduQuest_Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<APIResponse>
{
    //[JsonIgnore]
    public string Id { get; set; }
    public string? Username { get; set; }
    public string? Phone { get; set; }
    public string Headline { get; set; }
    public string Description { get; set; }

}
