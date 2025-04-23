using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Revenue.Query.GetChartRevenue
{
	public class GetChartRevenueQueryHandler : IRequestHandler<GetChartRevenueQuery, APIResponse>
	{
		private readonly ITransactionDetailRepository _transactionDetailRepository;

		public GetChartRevenueQueryHandler(ITransactionDetailRepository transactionDetailRepository)
		{
			_transactionDetailRepository = transactionDetailRepository;
		}

		public async Task<APIResponse> Handle(GetChartRevenueQuery request, CancellationToken cancellationToken)
		{
			var (earnings, sales, refunds) = await _transactionDetailRepository.GetChartRevenue(request.UserId);

			var response = new
			{
				Earnings = earnings,
				Sales = sales,
				Refunds = refunds
			};
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", "Chart Revenue");
		}
	}
}
