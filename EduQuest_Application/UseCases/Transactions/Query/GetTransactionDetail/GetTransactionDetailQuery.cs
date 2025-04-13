using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Transactions.Query.GetTransactionDetailByUserId
{
	public class GetTransactionDetailQuery : IRequest<APIResponse>
	{
        public string TransactionId { get; set; }

		public GetTransactionDetailQuery(string transactionId)
		{
			TransactionId = transactionId;
		}
	}
}
