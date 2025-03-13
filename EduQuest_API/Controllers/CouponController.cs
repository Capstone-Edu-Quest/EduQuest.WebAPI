using EduQuest_Application.DTO.Request.Coupons;
using EduQuest_Application.DTO.Response.Feedbacks;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Coupons.Commands.CreateCourseCoupons;
using EduQuest_Application.UseCases.Coupons.Commands.UpdateCourseCoupons;
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


    [Authorize(Roles = "Staff")]
    [HttpPost]
    public async Task<IActionResult> CreateCoupon([FromBody, Required] CreateCouponRequest coupon,
        //[FromQuery] string UserId,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new CreateCouponCommand(userId, coupon), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPut]
    public async Task<IActionResult> UpdateCoupon([FromQuery, Required] string couponId,
        [FromBody, Required] UpdateCouponRequest coupon,
        //[FromQuery] string UserId,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new UpdateCouponCommand(userId, couponId, coupon), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }



    [Authorize(Roles = "Staff")]
    [HttpGet]
    public async Task<IActionResult> GetPlatformCoupons([FromQuery, AllowNull] string code,
        [FromQuery, AllowNull] double discount,
        [FromQuery, AllowNull] DateTime expiredTime,
        //[FromQuery] string UserId,
        [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new GetPlatformCouponsQuery(userId, pageNo, eachPage, discount, code, expiredTime), token);
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
