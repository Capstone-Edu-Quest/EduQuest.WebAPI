using EduQuest_Application.DTO.Request.Coupons;
using EduQuest_Application.DTO.Response.Feedbacks;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Coupons.Commands.CreateCourseCoupons;
using EduQuest_Application.UseCases.Coupons.Commands.CreatePlatformCoupons;
using EduQuest_Application.UseCases.Coupons.Commands.UpdateCourseCoupons;
using EduQuest_Application.UseCases.Coupons.Queries.GetCourseCoupons;
using EduQuest_Application.UseCases.Coupons.Queries.GetPlatformCouponsQuery;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

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

    [Authorize(Roles = "Instructor")]
    [HttpGet("course")]
    public async Task<IActionResult> GetCourseCoupons(
        [FromQuery, AllowNull] string couponCode,
        [FromQuery, AllowNull] double discountValue,
        [FromQuery, AllowNull] DateTime expiredAt,
        [FromQuery, Required] string courseId,
        //[FromQuery] string UserId,
        [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new GetCourseCouponsQuery(userId, courseId, pageNo, eachPage, discountValue, couponCode, expiredAt), token);
        if ((result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK))
        {
            return BadRequest(result);
        }
        PagedList<FeedbackResponse> list = (PagedList<FeedbackResponse>)result.Payload!;
        Response.Headers.Add("X-Total-Element", list.TotalItems.ToString());
        Response.Headers.Add("X-Total-Page", list.TotalPages.ToString());
        Response.Headers.Add("X-Current-Page", list.CurrentPage.ToString());
        return Ok(result);
    }

    [Authorize(Roles = "Instructor")]
    [HttpPost("course")]
    public async Task<IActionResult> CreateCourseCoupon([FromBody, Required] CreateCouponRequest coupon,
        //[FromQuery] string UserId,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new CreateCourseCouponCommand(userId, coupon), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

    [Authorize(Roles = "Instructor")]
    [HttpPut("course")]
    public async Task<IActionResult> UpdateCourseCoupon([FromQuery, Required] string couponId,
        [FromBody, Required] UpdateCouponRequest coupon,
        //[FromQuery] string UserId,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new UpdateCourseCouponCommand(userId, couponId, coupon), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreatePlatformCoupon([FromBody, Required] CreatePlatformCouponRequest coupon,
        //[FromQuery] string UserId,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new CreatePlatformCouponCommand(userId, coupon), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetPlatformCoupons([FromQuery, AllowNull] string couponCode,
        [FromQuery, AllowNull] double discountValue,
        [FromQuery, AllowNull] DateTime expiredAt,
        //[FromQuery] string UserId,
        [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new GetPlatformCouponsQuery(userId, pageNo, eachPage, discountValue, couponCode, expiredAt), token);
        if ((result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK))
        {
            return BadRequest(result);
        }
        PagedList<FeedbackResponse> list = (PagedList<FeedbackResponse>)result.Payload!;
        Response.Headers.Add("X-Total-Element", list.TotalItems.ToString());
        Response.Headers.Add("X-Total-Page", list.TotalPages.ToString());
        Response.Headers.Add("X-Current-Page", list.CurrentPage.ToString());
        return Ok(result);
    }
}
