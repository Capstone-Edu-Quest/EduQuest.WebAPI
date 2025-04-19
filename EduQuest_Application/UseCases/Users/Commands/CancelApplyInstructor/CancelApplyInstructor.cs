using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetInstructorApplication;

public class CancelApplyInstructor : IRequest<APIResponse>
{
    public string userId { get; set; }
    public bool isCanceled { get; set; }
}
