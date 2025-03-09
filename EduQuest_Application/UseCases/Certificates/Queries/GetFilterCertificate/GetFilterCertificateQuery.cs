using EduQuest_Domain.Models.Response;
using MediatR;

public class GetCertificatesQuery : IRequest<APIResponse>
{
    public int Page { get; set; } = 1; 
    public int EachPage { get; set; } = 10; 
    public string? Title { get; set; }
    public string? UserId { get; set; }
    public string? CourseId { get; set; }
    public string? Url { get; set; }

    public GetCertificatesQuery()
    {
    }

    public GetCertificatesQuery(int? page, int? eachPage, string? title, string? userId, string? courseId, string? url)
    {
        Page = page ?? 1; 
        EachPage = eachPage ?? 10;
        Title = title;
        UserId = userId;
        CourseId = courseId;
        Url = url;
    }
}
