using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EduQuest_Infrastructure.Repository;

public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
{
    private readonly ApplicationDbContext _context;
    public CouponRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistByCode(string code)
    {
        return await _context.Coupon.Where(c => c.Usage <= c.Limit)
            .AnyAsync(x => x.Code == code);
    }

    public async Task<PagedList<Coupon>> GetAllPlatformCoupon(int pageNo, int pageSize, double? discountValue, string? couponCode, DateTime? expiredAt)
    {
        var result = _context.Coupon.Include(c => c.User).AsQueryable();

        if (discountValue.HasValue)
        {
            result = from r in result
                     where r.Discount >= (decimal)discountValue.Value
                     select r;
        }
        if (expiredAt.HasValue)
        {
            result = from r in result
                     where r.ExpireTime >= expiredAt.Value
                     select r;
        }
        if (!string.IsNullOrWhiteSpace(couponCode))
        {
            result = from r in result
                     where r.Code.Contains(couponCode!)
                     select r;
        }
        return await result.Pagination(pageNo, pageSize).ToPagedListAsync(pageNo, pageSize);
    }
    public async Task<bool> IsCouponAvailable(string code, string userId)
    {
        Coupon? coupon = await _context.Coupon.FirstOrDefaultAsync(c => c.Code == code);

        if (coupon == null || coupon.Usage >= coupon.Limit)
        {
            return false;
        }
        var temp = coupon.UserCoupons!.FirstOrDefault(uc => uc.UserId == userId);
        if(temp == null || temp.AllowUsage >= temp.RemainUsage)
        {
            return false;
        }
        return true;
    }
    public async Task<bool> ConsumeCoupon(string code, string userId)
    {
        Coupon? coupon = await _context.Coupon.FirstOrDefaultAsync(c => c.Code == code);

        if (coupon == null || coupon.Usage >= coupon.Limit)
        {
            return false;
        }
        var temp = coupon.UserCoupons!.FirstOrDefault(uc => uc.UserId == userId);
        if (temp == null)
        {
            UserCoupon newUserCoupon = new UserCoupon
            {
                UserId = userId,
                CouponId = coupon.Id,
                AllowUsage = coupon.Limit,
                RemainUsage = coupon.AllowUsagePerUser,
            };
            coupon.UserCoupons!.Add(newUserCoupon);
            return true;
        }
        else
        {
            int remain = temp.RemainUsage;
            remain -= 1;
            temp.RemainUsage = remain;
            return true;
        }
    }
}
