using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Revenue.Query.GetRevenueTransaction
{
	public class GetRevenueTransactionQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

		public GetRevenueTransactionQuery(string userId, DateTime? dateFrom, DateTime? dateTo)
		{
			UserId = userId;
			DateFrom = dateFrom;
			DateTo = dateTo;
		}
	}
}
