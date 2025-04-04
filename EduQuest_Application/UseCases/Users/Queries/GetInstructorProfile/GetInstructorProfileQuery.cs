using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetInstructorProfile;

public class GetInstructorProfileQuery : IRequest<APIResponse>
{
    public string userId { get; set; }
}
