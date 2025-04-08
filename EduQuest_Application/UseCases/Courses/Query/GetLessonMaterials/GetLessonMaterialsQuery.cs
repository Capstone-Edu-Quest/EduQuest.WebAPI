using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Query.GetLessonMaterials;

public class GetLessonMaterialsQuery : IRequest<APIResponse>
{
    public string LessonId { get; set; }
    public string UserId { get; set; }

    public GetLessonMaterialsQuery(string lessonId, string userId)
    {
        LessonId = lessonId;
        UserId = userId;
    }
}
