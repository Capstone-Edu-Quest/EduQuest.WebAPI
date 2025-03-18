using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.CartItems.Command;
using EduQuest_Application.UseCases.Carts.Query;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/cart")]

	public class CartController : BaseController
	{
		private ISender _mediator;


		public CartController(ISender mediator)
		{
			_mediator = mediator;

		}

		[HttpGet("")]
		public async Task<IActionResult> Register(CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();

			var result = await _mediator.Send(new GetCartByUserIdQuery(userId), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}


		[HttpPost("add-cartItem")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]

		public async Task<ActionResult<APIResponse>> AddCartItem([FromBody] List<string> courseIds, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();

			var result = await _mediator.Send(new AddCartItemCommand(userId, courseIds), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}
	}
}
