using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Revenue.Query.GetRevenueTransaction
{
	public class GetRevenueTransactionQueryHandler : IRequestHandler<GetRevenueTransactionQuery, APIResponse>
	{
		private readonly ITransactionDetailRepository _transactionDetailRepository;
		public Task<APIResponse> Handle(GetRevenueTransactionQuery request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
