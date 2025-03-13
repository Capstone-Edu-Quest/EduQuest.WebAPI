namespace EduQuest_Application.DTO.Request.Coupons;

public class UpdateCouponRequest
{
    public decimal Discount { get; set; }
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? ExpireTime { get; set; }
    public int AllowUsagePerUser { get; set; }
    public int Limit { get; set; }

    public List<string>? WhiteListCourseIds { get; set; }
    public List<string>? WhiteListUserIds { get; set; }
}
