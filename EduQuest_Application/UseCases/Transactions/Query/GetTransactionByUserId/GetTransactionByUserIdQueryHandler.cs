using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Transactions.Query.GetTransactionByUserId
{
	public class GetTransactionByUserIdQueryHandler : IRequestHandler<GetTransactionByUserIdQuery, APIResponse>
	{
		private readonly ITransactionRepository _transactionRepository;

		public GetTransactionByUserIdQueryHandler(ITransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		public async Task<APIResponse> Handle(GetTransactionByUserIdQuery request, CancellationToken cancellationToken)
		{
			var transaction = await _transactionRepository.GetTransactionByUserId(request.UserId);
			return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully,
				transaction, "name", "Transaction");
		}
	}
}
