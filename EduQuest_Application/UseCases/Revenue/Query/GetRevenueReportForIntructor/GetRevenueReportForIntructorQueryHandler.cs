using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Revenue.Query.GetRevenueReportForIntructor
{
	public class GetRevenueReportForIntructorQueryHandler : IRequestHandler<GetRevenueReportForIntructorQuery, APIResponse>
	{
		private readonly ITransactionDetailRepository _transactionDetailRepository;

		public GetRevenueReportForIntructorQueryHandler(ITransactionDetailRepository transactionDetailRepository)
		{
			_transactionDetailRepository = transactionDetailRepository;
		}

		public async Task<APIResponse> Handle(GetRevenueReportForIntructorQuery request, CancellationToken cancellationToken)
		{
			var result = await _transactionDetailRepository.GetRevenueReportAsync(request.UserId);
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, result, "name", $"Revenue of User ID {request.UserId}");
		}

	}
}
