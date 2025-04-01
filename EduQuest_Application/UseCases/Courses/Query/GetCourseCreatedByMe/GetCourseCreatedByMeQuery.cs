using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Queries
{
	public class GetCourseCreatedByMeQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }
		public int PageNo { get; set; }
		public int EachPage { get; set; }

		public GetCourseCreatedByMeQuery(string userId, int pageNo, int eachPage)
		{
			UserId = userId;
			PageNo = pageNo;
			EachPage = eachPage;
		}
	}
}
