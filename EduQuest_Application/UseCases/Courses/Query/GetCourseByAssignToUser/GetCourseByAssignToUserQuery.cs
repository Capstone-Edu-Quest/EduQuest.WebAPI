using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Query.GetCourseByAssignToUser;

public class GetCourseByAssignToUserQuery : IRequest<APIResponse>
{
    public string expertId { get; set; }
    public int PageNo { get; set; } = 1;
    public int EachPage { get; set; } = 10;

    public GetCourseByAssignToUserQuery(string expertId, int pageNo, int eachPage)
    {
        this.expertId = expertId;
        PageNo = pageNo;
        EachPage = eachPage;
    }
}
