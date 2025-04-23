using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Revenue.Query.GetChartRevenue
{
	public class GetChartRevenueQuery: IRequest<APIResponse>
	{
        public string UserId { get; set; }

		public GetChartRevenueQuery(string userId)
		{
			UserId = userId;
		}
	}
}
