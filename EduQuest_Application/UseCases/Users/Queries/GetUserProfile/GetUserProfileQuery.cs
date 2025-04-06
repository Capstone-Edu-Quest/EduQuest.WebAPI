using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetUserProfile;

public class GetUserProfileQuery : IRequest<APIResponse>
{
    public string? userId { get; set; }

}
