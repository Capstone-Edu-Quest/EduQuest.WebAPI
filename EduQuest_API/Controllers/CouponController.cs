using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/coupon")]
[ApiController]
public class CouponController : ControllerBase
{
    private ISender _mediator;

    public CouponController(ISender mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("course")]
    public async Task<IActionResult> GetCourseCoupons([FromQuery,Required] string courseId, 
        CancellationToken token = default)
    {
        //string userId = User.GetUserIdFromToken().ToString();
        throw new NotImplementedException();
    }
    [HttpPost("course")]
    public async Task<IActionResult> CreateCourseCoupon([FromBody, Required] string coupon,
        CancellationToken token = default)
    {
        //string userId = User.GetUserIdFromToken().ToString();
        throw new NotImplementedException();
    }
    [HttpPut("course")]
    public async Task<IActionResult> UpdateCourseCoupon([FromBody, Required] string coupon,
        CancellationToken token = default)
    {
        //string userId = User.GetUserIdFromToken().ToString();
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlatformCoupon([FromBody, Required] string coupon, 
        CancellationToken token = default)
    {
        //string userId = User.GetUserIdFromToken().ToString();
        throw new NotImplementedException();
    }
    [HttpGet]
    public async Task<IActionResult> GetPlatformCoupons(CancellationToken token = default)
    {
        //string userId = User.GetUserIdFromToken().ToString();
        throw new NotImplementedException();
    }
}
