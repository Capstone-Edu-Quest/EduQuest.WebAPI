using EduQuest_Application.UseCases.Expert.Commands.ApproveCourse;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/expert")]
[ApiController]
public class ExpertController : ControllerBase
{
    private ISender _mediator;

    public ExpertController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApprovePendingCourse([FromBody] ApproveCourseCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

}
