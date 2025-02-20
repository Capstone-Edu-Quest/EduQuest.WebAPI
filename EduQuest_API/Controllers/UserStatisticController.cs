
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Users.Queries.GetAllUsers;
using EduQuest_Application.UseCases.Users.Queries.GetCurrentUser;
using EduQuest_Application.UseCases.Users.Queries.GetUserGameInfo;
using EduQuest_Application.UseCases.UserStatistics.Commands.UpdateUsersStreak;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/userStatistic")]
public class UserStatisticController : BaseController
{
    private ISender _mediator;
    public UserStatisticController(ISender mediator)
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

}
