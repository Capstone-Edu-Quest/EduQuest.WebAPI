using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Revenue.Query.GetRevenueForAdmin
{
	public class GetRevenueForAdminQueryHandler : IRequestHandler<GetRevenueForAdminQuery, APIResponse>
	{
		private readonly ITransactionRepository _transactionRepository;

		public GetRevenueForAdminQueryHandler(ITransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		public async Task<APIResponse> Handle(GetRevenueForAdminQuery request, CancellationToken cancellationToken)
		{
			var response = await _transactionRepository.GetRevenueForAdminAsync(request.Request);

			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", "Revenue Transaction For Admin");
		}
	}
}
