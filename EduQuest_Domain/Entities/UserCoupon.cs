
using System.ComponentModel.DataAnnotations.Schema;


namespace EduQuest_Domain.Entities;

[Table("UserCoupon")]
public partial class UserCoupon
{
    public string UserId { get; set; }
    public string CouponId { get; set; }
    public int AllowUsage { get; set; }
    public int RemainUsage { get; set; }

    public virtual Coupon Coupon { get; set; }
    public virtual User User { get; set; }
}
