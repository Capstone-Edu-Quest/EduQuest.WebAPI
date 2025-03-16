using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;

public class Coupon : BaseEntity
{
    public decimal Discount { get; set; }
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? ExpireTime { get; set; }
    public int AllowUsagePerUser { get; set; }
    public int Usage {  get; set; }
    public int Limit {  get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    /*[JsonIgnore]
    public virtual ICollection<Course>? Course { get; set; }*/

    [JsonIgnore]
    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual ICollection<UserCoupon>? UserCoupons { get; set; }

    /*[JsonIgnore]
    public virtual ICollection<User>? WhiteListUsers { get; set; }*/
}
