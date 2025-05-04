using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetByCourseId;

public class GetByCourseIdQuery : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string CourseId { get; set;}

    public GetByCourseIdQuery(string userId, string courseId)
    {
        UserId = userId;
        CourseId = courseId;
    }
}
