using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Certificates.Queries.GetFilterCertificate;

public class GetCertificatesQuery : IRequest<APIResponse>
{
    public int? Page { get; set; }
    public int? EachPage { get; set; }
    public string? Title { get; set; }
    public string? UserId { get; set; }
    public string? CourseId { get; set; }
    public string? Url { get; set; }

    public GetCertificatesQuery()
    {
    }

    public GetCertificatesQuery(int? page, int? eachPage, string? title, string? userId, string? courseId, string? url)
    {
        Page = page;
        EachPage = eachPage;
        Title = title;
        UserId = userId;
        CourseId = courseId;
        Url = url;
    }
}
