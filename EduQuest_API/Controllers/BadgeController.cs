using EduQuest_Application.UseCases.Badges.Commands.CreateBadge;
using EduQuest_Application.UseCases.Badges.Commands.UpdateBadge;
using EduQuest_Application.UseCases.Badges.Queries;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/badge")]
public class BadgeController : BaseController
{
    private readonly ISender _mediator;

    public BadgeController(ISender mediator)
    {
        _mediator = mediator;
    }

    // [Authorize] 
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBadge([FromBody] CreateBadgeCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    // [Authorize] 
    [HttpPut("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBadge([FromBody] UpdateBadgeCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    // [Authorize] 
    [HttpGet("filter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBadges([FromQuery] GetBadgesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
