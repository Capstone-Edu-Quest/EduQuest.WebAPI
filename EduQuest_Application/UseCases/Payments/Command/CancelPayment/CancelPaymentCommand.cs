using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Payments.Command.CancelPayment
{
	public class CancelPaymentCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }

		public CancelPaymentCommand(string userId)
		{
			UserId = userId;
		}
	}
}
