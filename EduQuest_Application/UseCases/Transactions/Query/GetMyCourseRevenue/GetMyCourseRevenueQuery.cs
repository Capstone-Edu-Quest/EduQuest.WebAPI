using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Transactions.Query.GetMyCourseRevenue
{
	public class GetMyCourseRevenueQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }

		public GetMyCourseRevenueQuery(string userId)
		{
			UserId = userId;
		}
	}
}
