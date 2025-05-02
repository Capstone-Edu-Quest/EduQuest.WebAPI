using EduQuest_Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EduQuest_Application.UseCases.Users.Commands.BecomeInstructor;

public class BecomeInstructorCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string Headline { get; set; }
    public string Description { get; set; }
    public string Phone { get; set; }
    public List<string?> Tag { get; set; }
    public List<IFormFile> CertificateFiles { get; set; } = new();
}
