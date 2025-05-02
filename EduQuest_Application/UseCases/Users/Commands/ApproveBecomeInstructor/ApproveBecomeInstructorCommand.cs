using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Commands.ApproveBecomeInstructor;

public class ApproveBecomeInstructorCommand : IRequest<APIResponse>
{
    public string UserId { get; set; } = null!;
    public bool isApprove { get; set; }
    public string? RejectedReason { get; set; }
}
