using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Certificates.Commands.CreateCertificate;

public class CreateCertificateCommand : IRequest<APIResponse>
{
    public string Title { get; set; }
    public string? Url { get; set; }
    public string CourseId { get; set; }
    public string UserId { get; set; }
}
