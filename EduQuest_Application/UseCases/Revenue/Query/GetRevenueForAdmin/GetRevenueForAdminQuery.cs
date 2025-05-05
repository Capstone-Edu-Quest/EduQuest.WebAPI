using EduQuest_Application.DTO.Request.Revenue;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Revenue.Query.GetRevenueForAdmin
{
	public class GetRevenueForAdminQuery : IRequest<APIResponse>
	{
        public RevenueTransactionForAdmin Request { get; set; }

		public GetRevenueForAdminQuery(RevenueTransactionForAdmin request)
		{
			Request = request;
		}
	}
}
