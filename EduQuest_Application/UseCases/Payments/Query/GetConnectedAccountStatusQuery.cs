using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Payments.Query
{
	public class GetConnectedAccountStatusQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }

		public GetConnectedAccountStatusQuery(string userId)
		{
			UserId = userId;
		}
	}
}
