using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Users.Queries.GetAllUsers;
using EduQuest_Application.UseCases.Users.Queries.GetCurrentUser;
using EduQuest_Application.UseCases.Users.Queries.GetUserGameInfo;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EduQuest_API.Controllers;

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


    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetAccountPreByEvent(CancellationToken cancellationToken = default)
    {
        string email = User.GetEmailFromToken().ToString();
        var result = await _mediator.Send(new GetCurrentUserQuery(email), cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("home-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetUserOverallInfo([FromQuery] GetUserGameInfoQuery info, CancellationToken cancellationToken = default)
    {
        string email = User.GetEmailFromToken().ToString();
        var result = await _mediator.Send(info, cancellationToken);
        return Ok(result);
    }
}
