using EduQuest_Application.UseCases.WebStatistics.Queries.AdminHomeDashboard;
using EduQuest_Domain.Repository;
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
    private ICouponRepository _couponRepository;
    public WebStatisticController(ISender mediator, ICouponRepository couponRepository)
    {
        _mediator = mediator;
        _couponRepository = couponRepository;

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
    [HttpGet("TestCoupon")]
    public async Task<IActionResult> TestCouponStatistic(CancellationToken token = default)
    {
        var result = await _couponRepository.CouponStatistics();
        return Ok(result);
    }
}
