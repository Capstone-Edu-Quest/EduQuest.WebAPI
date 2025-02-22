using EduQuest_Application.DTO.Request.Coupons;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Coupons.Commands.UpdateCourseCoupons;

public class UpdateCourseCouponCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string CouponId { get; set;}
    public UpdateCouponRequest Coupon { get; set; }

    public UpdateCourseCouponCommand(string userId, string couponId, UpdateCouponRequest coupon)
    {
        UserId = userId;
        CouponId = couponId;
        Coupon = coupon;
    }
}
