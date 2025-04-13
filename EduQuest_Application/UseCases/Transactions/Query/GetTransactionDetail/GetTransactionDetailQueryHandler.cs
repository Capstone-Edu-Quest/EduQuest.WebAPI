using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Transactions.Query.GetTransactionDetailByUserId;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Transactions.Query.GetTransactionDetail
{
	public class GetTransactionDetailQueryHandler : IRequestHandler<GetTransactionDetailQuery, APIResponse>
	{
		private readonly ITransactionDetailRepository _transactionDetailRepository;

		public GetTransactionDetailQueryHandler(ITransactionDetailRepository transactionDetailRepository)
		{
			_transactionDetailRepository = transactionDetailRepository;
		}

		public async Task<APIResponse> Handle(GetTransactionDetailQuery request, CancellationToken cancellationToken)
		{
			var transactionDetailList = await _transactionDetailRepository.GetByTransactionId(request.TransactionId);
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, MessageCommon.GetSuccesfully, "name", $"Transactions Detail");
		}
	}
}
