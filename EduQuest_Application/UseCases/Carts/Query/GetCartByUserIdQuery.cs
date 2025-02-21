using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Carts.Query
{
	public class GetCartByUserIdQuery : IRequest<APIResponse>
	{
		public string UserId { get; set; }

		public GetCartByUserIdQuery(string userId)
		{
			UserId = userId;
		}
	}
}
