using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Transactions.Query.GetMyCourseRevenue
{
	public class GetMyCourseRevenueQueryHandler : IRequestHandler<GetMyCourseRevenueQuery, APIResponse>
	{
		//private readonly ICourseRepository _courseRepository;
		//public async Task<APIResponse> Handle(GetMyCourseRevenueQuery request, CancellationToken cancellationToken)
		//{
		//	var myListCourse = await _courseRepository.GetCoursesByInstructorIdAsync(request.UserId);
		//}
		public Task<APIResponse> Handle(GetMyCourseRevenueQuery request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
