using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Feedbacks;

public class FeedbackResponse : IMapFrom<Feedback>, IMapTo<Feedback>
{
    public string Id { get; set; } = string.Empty;
    public string CourseId { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public CommonUserResponse CreatedBy {  get; set; } = new CommonUserResponse();
}
