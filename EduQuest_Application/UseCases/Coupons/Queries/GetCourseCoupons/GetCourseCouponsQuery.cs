using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Coupons.Queries.GetCourseCoupons;

public class GetCourseCouponsQuery : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string CourseId { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public double? DiscountValue { get; set; }
    public string? CouponCode { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public GetCourseCouponsQuery(string userId, string courseId, int pageNo, int pageSize,
        double? discountValue, string? couponCode, DateTime? expiredAt)
    {
        UserId = userId;
        CourseId = courseId;
        PageNo = pageNo;
        PageSize = pageSize;
        DiscountValue = discountValue;
        CouponCode = couponCode;
        ExpiredAt = expiredAt;
    }
}
