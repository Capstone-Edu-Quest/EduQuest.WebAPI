using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.UserMetas.Commands.UpdateUserProgress;
using EduQuest_Application.UseCases.UserMetas.Commands.UpdateUsersStreak;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
	public async Task<IActionResult> UpdateUserProgress([FromBody] UpdateUserProgressCommand command, CancellationToken cancellationToken = default)
	{
		string userId = User.GetUserIdFromToken().ToString();
		var result = await _mediator.Send(new UpdateUserProgressCommand(userId, command.CourseId, command.MaterialId, command.Time), cancellationToken);
		return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
	}

}
