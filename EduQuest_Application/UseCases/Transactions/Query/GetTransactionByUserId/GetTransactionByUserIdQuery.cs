using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Transactions.Query.GetTransactionByUserId
{
	public class GetTransactionByUserIdQuery: IRequest<APIResponse>
	{
        public string UserId { get; set; }

		public GetTransactionByUserIdQuery(string userId)
		{
			UserId = userId;
		}
	}
}
