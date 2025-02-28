using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
