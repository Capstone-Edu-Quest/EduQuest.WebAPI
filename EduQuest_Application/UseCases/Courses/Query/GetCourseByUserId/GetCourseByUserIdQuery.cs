using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Queries
{
	public class GetCourseByUserIdQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public string IntructorId { get; set; }
		public int PageNo { get; set; }
		public int EachPage { get; set; }

		public GetCourseByUserIdQuery(string userId, string intructorId, int pageNo, int eachPage)
		{
			UserId = userId;
			IntructorId = intructorId;
			PageNo = pageNo;
			EachPage = eachPage;
		}
	}
}
