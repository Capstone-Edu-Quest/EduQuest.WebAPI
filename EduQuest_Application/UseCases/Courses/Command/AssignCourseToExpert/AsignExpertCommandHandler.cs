using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Command.AssignCourseToExpert;

public class AsignExpertCommandHandler : IRequestHandler<AssignExpertCommand, APIResponse>
{
    public Task<APIResponse> Handle(AssignExpertCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
