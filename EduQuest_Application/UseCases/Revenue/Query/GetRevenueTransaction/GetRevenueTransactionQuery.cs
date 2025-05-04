using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Revenue.Query.GetRevenueTransaction
{
	public class GetRevenueTransactionQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }

		public GetRevenueTransactionQuery(string userId)
		{
			UserId = userId;
		}
	}
}
