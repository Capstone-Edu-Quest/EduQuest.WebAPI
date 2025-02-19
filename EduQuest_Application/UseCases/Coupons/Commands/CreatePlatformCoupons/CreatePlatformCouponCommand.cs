

using EduQuest_Application.DTO.Request.Coupons;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Coupons.Commands.CreatePlatformCoupons;

public class CreatePlatformCouponCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public CreatePlatformCouponRequest Coupon { get; set; }

    public CreatePlatformCouponCommand(string userId, CreatePlatformCouponRequest coupon)
    {
        UserId = userId;
        Coupon = coupon;
    }
}
