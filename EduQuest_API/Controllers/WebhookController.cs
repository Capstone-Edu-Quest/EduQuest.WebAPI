using EduQuest_Application.UseCases.Transactions.Command.UpdateTransactionStatus;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Payment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_API.Controllers
{
	[Route("api/webhook/stripe")]
	public class WebhookController : ControllerBase
	{
		private ISender _mediator;
		private readonly StripeModel _stripeModel;

		public WebhookController(ISender mediator, IOptions<StripeModel> stripeModel)
		{
			_mediator = mediator;
			_stripeModel = stripeModel.Value;
		}

		[HttpPost]
		public async Task<IActionResult> StripeWebhook(CancellationToken cancellationToken)
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			Event stripeEvent;

			try
			{
				stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _stripeModel.SecretKey);
			}
			catch (StripeException e)
			{
				return BadRequest($"⚠️ Webhook signature verification failed: {e.Message}");
			}

			if (stripeEvent.Type == "checkout.session.completed")
			{
				var session = stripeEvent.Data.Object as Session;
				if (session != null)
				{
					await _mediator.Send(new UpdateTransactionStatusCommand
					{
						TransactionId = session.Id,
						Status = StatusPayment.Completed.ToString()
					}, cancellationToken);
				}
			}
			else if (stripeEvent.Type == "checkout.session.expired")
			{
				var session = stripeEvent.Data.Object as Session;
				if (session != null)
				{
					await _mediator.Send(new UpdateTransactionStatusCommand
					{
						TransactionId = session.Id,
						Status = StatusPayment.Expired.ToString()
					});
				}
			}

			return Ok();
		}
	}
}
