using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetLearnerProfile;

public class GetLearnerProfileQuery : IRequest<APIResponse>
{
    public string? userId { get; set; }
}
