using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.WebStatistics.Queries.AdminHomeDashboard;
using EduQuest_Application.UseCases.WebStatistics.Queries.GetStatisticForInstructor;
using EduQuest_Application.UseCases.WebStatistics.Queries.StaffStatistics;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/webStatistic")]
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

	[HttpGet("instructor/home")]
	public async Task<IActionResult> GetHomeStatisticForInstructor(CancellationToken token = default)
	{
        string userId = User.GetUserIdFromToken().ToString();
		var result = await _mediator.Send(new GetStatisticForInstructorQuery(userId), token);
		return Ok(result);
	}

	[HttpGet("platform/setting")]
    public async Task<IActionResult> PatformStatistic(CancellationToken token = default)
    {
        var result = await _mediator.Send(new GetStaffStatisticQuery(), token);
        return Ok(result);
    }
    
}
