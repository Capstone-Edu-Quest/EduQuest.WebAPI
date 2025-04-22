using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Revenue.Query.GetCourseRevenue
{
	public class GetCourseRevenueQueryHandler : IRequestHandler<GetCourseRevenueQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly ITransactionDetailRepository _transactionDetailRepository;

		public GetCourseRevenueQueryHandler(ICourseRepository courseRepository, ITransactionDetailRepository transactionDetailRepository)
		{
			_courseRepository = courseRepository;
			_transactionDetailRepository = transactionDetailRepository;
		}

		public async Task<APIResponse> Handle(GetCourseRevenueQuery request, CancellationToken cancellationToken)
		{
			var myCourseIds = (await _courseRepository.GetCourseByUserId(request.UserId)).Select(x => x.Id).Distinct().ToList();
			var response = await _transactionDetailRepository.GetCourseRevenue(myCourseIds);
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", "Course Revenue");
		}
	}
}
