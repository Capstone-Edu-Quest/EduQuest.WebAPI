using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/subcription")]
	public class SubscriptionController : BaseController
	{
		private ISender _mediator;
		public SubscriptionController(ISender mediator)
		{
			_mediator = mediator;
		}

		//[HttpGet("")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> GetAllSubscription(CancellationToken cancellationToken = default)
		//{
		//	string userId = User.GetUserIdFromToken().ToString();
		//	var result = await _mediator.Send(new GetSubscriptionsQuery(), cancellationToken);
		//	return Ok(result);
		//}
	}
}
