using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Certificates.Commands.CreateCertificate;

public class CreateCertificateCommand : IRequest<APIResponse>
{
    public string CourseId { get; set; }
    public string UserId { get; set; }

    public CreateCertificateCommand(string courseId, string userId)
    {
        CourseId = courseId;
        UserId = userId;
    }
}
