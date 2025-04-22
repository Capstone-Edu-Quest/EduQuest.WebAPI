using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Revenue.Query.GetCourseRevenue
{
	public class GetCourseRevenueQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }

		public GetCourseRevenueQuery(string userId)
		{
			UserId = userId;
		}
	}
}
