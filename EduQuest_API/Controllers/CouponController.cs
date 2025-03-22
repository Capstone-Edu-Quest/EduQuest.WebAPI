using EduQuest_Application.DTO.Request.Coupons;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Coupons.Commands.CreateCourseCoupons;
using EduQuest_Application.UseCases.Coupons.Commands.UpdateCourseCoupons;
using EduQuest_Application.UseCases.Coupons.Queries.GetPlatformCouponsQuery;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace EduQuest_API.Controllers;


[Authorize(Roles = "Staff, Admin")]
[Route(Constants.Http.API_VERSION + "/coupon")]
[ApiController]
public class CouponController : ControllerBase
{
    private ISender _mediator;
    //private readonly ICouponRepository _couponRepository;
    public CouponController(ISender mediator, /*ICouponRepository couponRepository*/)
    {
        _mediator = mediator;
        //_couponRepository = couponRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCoupon([FromBody, Required] CreateCouponRequest coupon,
        //[FromQuery] string userId,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new CreateCouponCommand(userId, coupon), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCoupon([FromQuery, Required] string couponId,
        [FromBody, Required] UpdateCouponRequest coupon,
        //[FromQuery] string userId,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new UpdateCouponCommand(userId, couponId, coupon), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetPlatformCoupons([FromQuery, AllowNull] string? code,
        [FromQuery, AllowNull] double discount,
        [FromQuery, AllowNull] DateTime expiredTime,
        //[FromQuery] string userId,
        [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new GetPlatformCouponsQuery(userId, pageNo, eachPage, discount, code, expiredTime), token);
        if ((result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK))
        {
            return BadRequest(result);
        }
        PagedList<CouponResponse> list = (PagedList<CouponResponse>)result.Payload!;
        Response.Headers.Add("X-Total-Element", list.TotalItems.ToString());
        Response.Headers.Add("X-Total-Page", list.TotalPages.ToString());
        Response.Headers.Add("X-Current-Page", list.CurrentPage.ToString());
        return Ok(result);
    }

    /*[HttpPost("test")]
    public async Task<IActionResult> testconsumecoupon([FromQuery] string couponCode,
        [FromQuery] string userId)
    {
        if (await _couponRepository.ConsumeCoupon(couponCode, userId))
        {           
                return Ok("success!");
        }
        return BadRequest("Error");
    }*/
}
