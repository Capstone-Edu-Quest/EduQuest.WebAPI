using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Certificates.Queries.GetFilterCertificate;

public class GetCertificatesQuery : IRequest<APIResponse>
{
    public string? Title { get; set; }
    public string? UserId { get; set; }
    public string? CourseId { get; set; }
    public string? Url { get; set; }

    public GetCertificatesQuery(string? title, string? userId, string? courseId, string? url)
    {
        Title = title;
        UserId = userId;
        CourseId = courseId;
        Url = url;
    }
}
