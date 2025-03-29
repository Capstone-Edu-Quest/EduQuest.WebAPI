using EduQuest_Application.UseCases.Certificates.Commands.CreateCertificate;
using EduQuest_Application.UseCases.Levels.Command.CreateLevel;
using EduQuest_Application.UseCases.Levels.Command.UpdateLevels;
using EduQuest_Application.UseCases.Levels.Query.GetFilterLevels;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/level")]
[ApiController]
public class LevelController : ControllerBase
{
    private ISender _mediator;

    public LevelController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("filter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFilterLevel([FromQuery] GetFilterLevelQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }



    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewLevel([FromBody] CreateLevelCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLevel([FromBody] UpdateLevelCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
