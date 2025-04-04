using EduQuest_Application.UseCases.WebStatistics.Queries.AdminHomeDashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EduQuest_API.Controllers;

[Route("api/webStatistic")]
[ApiController]
//[Authorize(Roles ="Admin")]
public class WebStatisticController : ControllerBase
{
    private ISender _mediator;
    public WebStatisticController(ISender mediator)
    {
        _mediator = mediator;

    }
    [HttpGet("user")]
    public async Task<IActionResult> GetWebUsersStatistic([FromQuery] string keyword,
        [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
        CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    [HttpGet("overall")]
    public async Task<IActionResult> GetWebStatistic(
        CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
    [HttpGet("admin/home")]
    public async Task<IActionResult> AdminDashboard(CancellationToken token = default)
    {
        var result = await _mediator.Send(new AdminHomeDashboardQuery(), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }
}
