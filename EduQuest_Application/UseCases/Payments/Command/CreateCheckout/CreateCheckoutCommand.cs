using EduQuest_Application.DTO.Request.Payment;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Payments.Command.CreateCheckout
{
	public class CreateCheckoutCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public string? CartId { get; set; }

		public CreateCheckoutCommand(string userId, string? cartId)
		{
			UserId = userId;
			CartId = cartId;
		}
	}
}
