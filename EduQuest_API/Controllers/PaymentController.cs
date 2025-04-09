using EduQuest_Application.DTO.Request.Payment;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Payments.Command.CreateCheckout;
using EduQuest_Application.UseCases.Payments.Command.Refund;
using EduQuest_Application.UseCases.Payments.Command.StripeExpress;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/payment")]
	public class PaymentController : BaseController
	{
		private readonly ISender _mediator;

		public PaymentController(ISender mediator)
		{
			_mediator = mediator;
		}

		[Authorize]
		[HttpPost("checkout")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Checkout([FromBody] CreateCheckoutRequest command, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new CreateCheckoutCommand(userId, command), cancellationToken);
			return Ok(result);
		}

		[HttpPost("stripeExpress")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateStripeExpress([FromBody] string email, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new StripeExpressCommand(email, userId), cancellationToken);
			return Ok(result);
		}

		[HttpPost("refund")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Refund([FromBody] RefundRequest request, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new RefundCommand(userId, request), cancellationToken);
			return Ok(result);
		}
	}
}
