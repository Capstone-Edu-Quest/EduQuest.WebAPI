﻿using EduQuest_Application.DTO.Request.Subscriptions;
using EduQuest_Application.UseCases.Subscriptions.Command.UpdateSubscription;
using EduQuest_Application.UseCases.Subscriptions.Query.GetSubscriptions;
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

		[HttpGet("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetAllSubscription(CancellationToken cancellationToken = default)
		{
			//string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetSubscriptionsQuery(), cancellationToken);
			return Ok(result);
		}

		//[Authorize]
		[HttpPut("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateAllSubscription([FromBody] List<SubscriptionRequest> subscriptions, CancellationToken cancellationToken = default)
		{
			//string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new UpdateSubscriptionCommand(subscriptions), cancellationToken);
			return Ok(result);
		}
	}
}
