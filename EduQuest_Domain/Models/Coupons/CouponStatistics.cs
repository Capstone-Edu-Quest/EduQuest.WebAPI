using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Models.Coupons;

public class CouponStatistics
{
    public int CreatedCoupons { get; set; } = 0;//direct query from main table Coupon
    public int ExpiredCoupons { get; set; } = 0;//direct query from main table Coupon
    public int RedeemedTimes { get; set; } = 0;//direct query from main table Coupon
    public List<CouponGraph> GraphData { get; set; } = new List<CouponGraph>();
}
