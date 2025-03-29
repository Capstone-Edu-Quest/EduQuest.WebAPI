using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Query.GetCourseByStatus;

public class GetCourseByStatusQuery : IRequest<APIResponse>
{
    public string Status { get; set; } = string.Empty;
    public int PageNo { get; set; } = 1;
    public int EachPage { get; set; } = 10;

   public GetCourseByStatusQuery(string status, int pageNo, int eachPage)
    {
        Status = status;
        PageNo = pageNo;
        EachPage = eachPage;
    }
    
    public GetCourseByStatusQuery()
    {
    }
    
}
