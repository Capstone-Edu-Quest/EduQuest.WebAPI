using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ICouponRepository : IGenericRepository<Coupon>
{
    Task<bool> ExistByCode(string code);
    Task<PagedList<Coupon>> GetAllPlatformCoupon(int pageNo, int pageSize,
        double? discountValue, string? couponCode, DateTime? expiredAt);

    // Counpon expire time < current time && usage < limit then return true.
    // Otherwise return false
    [Obsolete("This method is deprecated. Use ConsumeCoupon instead.", false)]
    Task<bool> IsCouponAvailable(string code, string userId);

    //Summarize
    //Consume success then return true. otherwise return false.
    //Consume a coupon increase it usage by 1 and mark a user that used the coupon as used by 2 field
    //in the UserCoupon entity: AllowUsage = Coupon.AllowUsagePerUser, RemainUsage = AllowUsage, when consume
    //RemainUsage is reduced by 1.
    Task<bool> ConsumeCoupon(string code, string userId);
    Task<Coupon> GetCouponByCode(string code);
}
