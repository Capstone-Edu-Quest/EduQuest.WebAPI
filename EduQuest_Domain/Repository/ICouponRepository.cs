using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ICouponRepository : IGenericRepository<Coupon>
{
    Task<bool> ExistByCode(string code);
    Task<PagedList<Coupon>> GetAllPlatformCoupon(int pageNo, int pageSize,
        double? discountValue, string? couponCode, DateTime? expiredAt);
}
