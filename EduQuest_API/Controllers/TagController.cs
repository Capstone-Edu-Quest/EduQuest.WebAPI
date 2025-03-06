using EduQuest_Application.UseCases.Certificates.Commands.CreateCertificate;
using EduQuest_Application.UseCases.Tags.Commands.CreateTag;
using EduQuest_Application.UseCases.Tags.Queries.GetFilterTag;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/tag")]
[ApiController]
public class TagController : ControllerBase
{
    private ISender _mediator;

    public TagController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("filter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFilterTag([FromQuery] GetFilterTagQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }



    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

}
