using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Transactions.Command.UpdateTransactionStatus;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Payment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nest;
using Stripe;
using Stripe.Checkout;
using System.Net;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/webhook/stripe")]
	[ApiController]
	public class WebhookController : ControllerBase
	{
		private ISender _mediator;
		private readonly StripeModel _stripeModel;
		

		public WebhookController(ISender mediator, IOptions<StripeModel> stripeModel)
		{
			_mediator = mediator;
			_stripeModel = stripeModel.Value;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> StripeWebhook(CancellationToken cancellationToken)
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			Event stripeEvent;

			stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _stripeModel.ProductionSigningKey);

			if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted || stripeEvent.Type == EventTypes.ChargeSucceeded || stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
			{
				var session = stripeEvent.Data.Object as Session;
				if (session != null)
				{
					
					var result = await _mediator.Send(new UpdateTransactionStatusCommand
					{
						TransactionId = session.Id,
						Status = StatusPayment.Completed.ToString(),
						PaymentIntentId = session.PaymentIntentId,
						CustomerEmail = session.CustomerDetails.Email,
						CustomerName = session.CustomerDetails.Name,
                    }, cancellationToken);
                    return Ok(result);
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
                        Status = StatusPayment.Expired.ToString(),
						PaymentIntentId = session.PaymentIntentId,
						CustomerEmail = session.CustomerDetails.Email,
						CustomerName = session.CustomerDetails.Name,
					});
				}
			}

			return Ok();
		}
	}
}
