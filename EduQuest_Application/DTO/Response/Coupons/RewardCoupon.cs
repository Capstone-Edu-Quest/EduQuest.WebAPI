using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Coupons;

public class RewardCoupon : IMapFrom<Coupon>
{
    public decimal Discount { get; set; }
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
}
