using EduQuest_Application.DTO.Request.Level;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Application.DTO.Response.Levels;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Certificates.Commands.CreateCertificate;
using EduQuest_Application.UseCases.Level.Command.CreateLevel;
using EduQuest_Application.UseCases.Level.Command.UpdateLevels;
using EduQuest_Application.UseCases.Level.Query.GetFilterLevels;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace EduQuest_API.Controllers;

[Authorize(Roles = "Staff, Admin")]
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
        PagedList<LevelResponseDto> list = (PagedList<LevelResponseDto>)result.Payload!;
        Response.Headers.Add("X-Total-Element", list.TotalItems.ToString());
        Response.Headers.Add("X-Total-Page", list.TotalPages.ToString());
        Response.Headers.Add("X-Current-Page", list.CurrentPage.ToString());
        return Ok(result);
    }



    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewLevel([FromBody] LevelDto level, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new CreateLevelCommand(level), cancellationToken);
        return Ok(result);
    }

    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLevel([FromBody] List<LevelDto> levels, CancellationToken cancellationToken = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new UpdateLevelCommand(userId, levels), cancellationToken);
        return Ok(result);
    }
}
