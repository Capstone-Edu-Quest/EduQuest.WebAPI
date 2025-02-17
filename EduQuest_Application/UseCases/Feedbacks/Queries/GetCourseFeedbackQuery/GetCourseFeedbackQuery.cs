

using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Feedbacks.Queries.GetCourseFeedbackQuery;

public class GetCourseFeedbackQuery : IRequest<APIResponse>
{
    public string courseId { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public int? Rating { get; set; }
    public string? Feedback {  get; set; }

    public GetCourseFeedbackQuery(string courseId, int pageNo, int pageSize, int? rating, string? feedback)
    {
        this.courseId = courseId;
        PageNo = pageNo;
        PageSize = pageSize;
        Rating = rating;
        Feedback = feedback;
    }
}
