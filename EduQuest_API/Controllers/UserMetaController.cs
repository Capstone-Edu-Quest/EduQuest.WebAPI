using EduQuest_Application.DTO.Request.UserMetas;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.UserMetas.Commands.UpdateUserProgress;
using EduQuest_Application.UseCases.UserMetas.Commands.UpdateUsersStreak;
using EduQuest_Application.UseCases.UserMetas.Queries.GetLeaderboard;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/userMeta")]
public class UserMetaController : BaseController
{
    private ISender _mediator;
    public UserMetaController(ISender mediator)
    {
        _mediator = mediator;

    }

    [HttpPut("streak")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUsersStreak([FromBody] UpdateUsersStreakCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

	[HttpPut("userProgress")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateUserProgress([FromBody] UpdateUserProgressRequest command, CancellationToken cancellationToken = default)
	{
		string userId = User.GetUserIdFromToken().ToString();
		var result = await _mediator.Send(new UpdateUserProgressCommand("7bd6f347-b27a-4f15-b8cd-b1fea1ab1fc9", command), cancellationToken);
		return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
	}
    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard(CancellationToken token = default)
    {
        var result = await _mediator.Send(new GetLeaderboardQuery(), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }
}
