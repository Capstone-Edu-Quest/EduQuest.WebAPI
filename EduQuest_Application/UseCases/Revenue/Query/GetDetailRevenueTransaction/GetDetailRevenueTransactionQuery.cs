using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Revenue.Query.GetDetailRevenueTransaction
{
	public class GetDetailRevenueTransactionQuery : IRequest<APIResponse>
	{
        public string Id { get; set; }

		public GetDetailRevenueTransactionQuery(string id)
		{
			Id = id;
		}
	}
}
