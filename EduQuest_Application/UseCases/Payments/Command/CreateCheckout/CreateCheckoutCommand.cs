using EduQuest_Application.DTO.Request.Payment;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Payments.Command.CreateCheckout
{
	public class CreateCheckoutCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public CreateCheckoutRequest Request { get; set; }

		public CreateCheckoutCommand(string userId, CreateCheckoutRequest request)
		{
			UserId = userId;
			Request = request;
		}
	}
}
