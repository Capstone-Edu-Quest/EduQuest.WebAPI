using EduQuest_Domain.Models.Response;
using MediatR;

public class GetCertificatesQuery : IRequest<APIResponse>
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? CourseId { get; set; }

}
