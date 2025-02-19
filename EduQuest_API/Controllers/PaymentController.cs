using EduQuest_Application.DTO.Request.Payment;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Payment.Command.CreateCheckout;
using EduQuest_Application.UseCases.Payment.Command.Refund;
using EduQuest_Application.UseCases.Payment.Command.StripeExpress;
using EduQuest_Domain.Constants;
using MediatR;
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

		[HttpPost("checkout")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Checkout([FromBody] List<ProductRequest> request, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new CreateCheckoutCommand(userId, request), cancellationToken);
			return Ok(result);
		}

		[HttpPost("stripeExpress")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateStripeExpress([FromBody] string email, CancellationToken cancellationToken = default)
		{
			//string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new StripeExpressCommand(email), cancellationToken);
			return Ok(result);
		}

		[HttpPost("refund")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Refund([FromBody] RefundRequest request, CancellationToken cancellationToken = default)
		{
			//string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new RefundCommand(request), cancellationToken);
			return Ok(result);
		}
	}
}
