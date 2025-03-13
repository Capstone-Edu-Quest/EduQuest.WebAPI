

using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Coupons;

public class CouponResponse : IMapFrom<Coupon>
{
    public string Id { get; set; } = string.Empty;
    public decimal Discount { get; set; }
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? ExpireTime { get; set; }
    public int AllowUsagePerUser { get; set; }
    public int Usage { get; set; }
    public int Limit { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<string>? WhiteListCourseIds { get; set; }
    public List<string>? WhiteListUserIds { get; set; }
    //public CommonUserResponse CreatedByUser { get; set; } = new CommonUserResponse();
}
