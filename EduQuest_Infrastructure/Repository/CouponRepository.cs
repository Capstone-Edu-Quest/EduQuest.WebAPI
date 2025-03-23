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

        if (coupon == null)
        {
            return false;
        }
        if(coupon.Usage >= coupon.Limit || coupon.ExpireTime < DateTime.Now)
        {
            return false;
        }
        var temp = coupon.UserCoupons!.FirstOrDefault(uc => uc.UserId == userId);
        if(temp!= null && temp!.RemainUsage < 1)
        {
            return false;
        }
        return true;
    }
    public async Task<bool> ConsumeCoupon(string code, string userId)
    {
        Coupon? coupon = await _context.Coupon
        .Include(c => c.UserCoupons)
        .FirstOrDefaultAsync(c => c.Code == code);

        if (coupon == null || coupon.Usage >= coupon.Limit || coupon.ExpireTime < DateTime.UtcNow)
        {
            return false;
        }
        var userCoupon = coupon.UserCoupons?.FirstOrDefault(uc => uc.UserId == userId);
        if (userCoupon == null)
        {
            userCoupon = new UserCoupon
            {
                UserId = userId,
                CouponId = coupon.Id,
                AllowUsage = coupon.AllowUsagePerUser,
                RemainUsage = coupon.AllowUsagePerUser - 1
            };
            coupon.UserCoupons?.Add(userCoupon);
        }
        else
        {
            if (userCoupon.RemainUsage <= 0)
            {
                return false;
            }
            userCoupon.RemainUsage -= 1;
        }
        coupon.Usage += 1;
        return await _context.SaveChangesAsync() > 0;
    }

	public async Task<Coupon> GetCouponByCode(string code)
	{
        return await _context.Coupon.FirstOrDefaultAsync(x => x.Code == code);
	}
}
