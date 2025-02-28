using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;

public class Coupon : BaseEntity
{
    public double DiscountValue { get; set; }
    public string Code { get; set; } = string.Empty;
    public bool IsCourseExclusive { get; set; }
    public string? CourseId { get; set; }
    public DateTime ExpireAt { get; set; }
    public int Usage {  get; set; }
    public int RemainUsage {  get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    [JsonIgnore]
    public virtual Course? Course { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual ICollection<UserCoupon> UsedByUsers { get; set; }
}
