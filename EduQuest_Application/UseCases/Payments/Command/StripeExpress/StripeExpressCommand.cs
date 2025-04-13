using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Payments.Command.StripeExpress
{
	public class StripeExpressCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }

		public StripeExpressCommand(string userId)
		{
			UserId = userId;
		}
	}
}
