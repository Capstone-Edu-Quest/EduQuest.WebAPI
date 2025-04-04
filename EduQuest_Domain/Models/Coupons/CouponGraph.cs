using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Models.Coupons;

public class CouponGraph
{
    public int RedeemTimes { get; set; } = 0;//UserCoupon Table: total Allow Usages - Remain Usages
    public int NewCoupons { get; set; } = 0;//Direct Query from Coupon table
    public int ExpiredCoupons { get; set; } = 0;//Direct Query from Coupon table
    public string Date {  get; set; } = string.Empty;//MMM yyyy
}
