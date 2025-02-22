

using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Coupons;

public class CouponResponse : IMapFrom<Coupon>
{
    public string Id { get; set; } = string.Empty;
    public double DiscountValue { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime ExpireAt { get; set; }
    public int Usage { get; set; }
    public int RemainUsage { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreateAt { get; set; }
    public CommonUserResponse CreatedByUser { get; set; } = new CommonUserResponse();
}
