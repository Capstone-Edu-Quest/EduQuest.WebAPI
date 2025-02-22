using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Coupons.Queries.GetPlatformCouponsQuery;

public class GetPlatformCouponsQuery : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public double? DiscountValue { get; set; }
    public string? CouponCode { get; set; }
    public DateTime? ExpiredAt { get; set; }

    public GetPlatformCouponsQuery(string userId, int pageNo, int pageSize, double? discountValue, 
        string? couponCode, DateTime? expiredAt)
    {
        UserId = userId;
        PageNo = pageNo;
        PageSize = pageSize;
        DiscountValue = discountValue;
        CouponCode = couponCode;
        ExpiredAt = expiredAt;
    }
}
