using EduQuest_Application.UseCases.Users.Queries.GetAllUsers;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/user")]
	public class UserController : BaseController
	{
		private ISender _mediator;
		public UserController(ISender mediator)
		{
			_mediator = mediator;

		}

		[HttpGet("all")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetAllUsers([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new GetAllUsersQuery(pageNo, eachPage), cancellationToken);
			return Ok(result);
		}
	}
}
