using EduQuest_Application.UseCases;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/message")]
	public class MessageController : BaseController
	{
		private readonly IMediator _mediator;

		public MessageController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("send")]
		public async Task<IActionResult> SendMessage([FromBody] SendMessageWithNotificationCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(result);
		}
	}
}
