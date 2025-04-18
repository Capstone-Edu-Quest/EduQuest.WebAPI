using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Commands.AssignInstructorToExpert;

public class AssignIntructorToExpert : IRequest<APIResponse>
{
    public string InstructorId { get; set; }
    public string AssignTo { get; set; }
}
