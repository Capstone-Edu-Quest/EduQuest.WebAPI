using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Coupons;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static EduQuest_Domain.Enums.GeneralEnums;

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
    public async Task<CouponStatistics> CouponStatistics()
    {
        CouponStatistics result = new CouponStatistics();
        var coupons = _context.Coupon.Include(c => c.UserCoupons).AsQueryable();
        result.CreatedCoupons = await coupons.CountAsync();
        result.ExpiredCoupons = await coupons.Where(c => c.ExpireTime <= DateTime.Now.ToUniversalTime()).CountAsync();
        result.RedeemedTimes = await coupons.SumAsync(c => c.Usage);
        result.GraphData = await CreateGraphDataCoupon(coupons);
        
        return result;
    }
    private async Task<List<CouponGraph>> CreateGraphDataCoupon(IQueryable<Coupon> coupons)
    {
        DateTime dateTemp = DateTime.Now;
        DateTime end = new DateTime(dateTemp.Year, dateTemp.Month + 1, 1);
        DateTime start = end.AddMonths(-6);
        var coupon = await coupons
            .Where(u => u.CreatedAt >= start.ToUniversalTime() && u.CreatedAt <= end.ToUniversalTime())
            .ToListAsync();

        //Month Group
        var result = Enumerable.Range(0, 6)
            .Select(i => new CouponGraph
            {
                Date = start.AddMonths(i).ToString("MMM yyyy"),
                
                NewCoupons = coupon.Count(c => c.CreatedAt.Value.Year == start.AddMonths(i).Year && c.CreatedAt.Value.Month == start.AddMonths(i).Month),
                
                ExpiredCoupons = coupon.Count(c => c.CreatedAt.Value.Year == start.AddMonths(i).Year && c.CreatedAt.Value.Month == start.AddMonths(i).Month 
                && c.ExpireTime.Value.Year == start.AddMonths(i).Year && c.ExpireTime.Value.Month == start.AddMonths(i).Month),
                
                RedeemTimes = coupon.Where(c => c.CreatedAt.Value.Year == start.AddMonths(i).Year && c.CreatedAt.Value.Month == start.AddMonths(i).Month)
                .Sum(c => c.UserCoupons.Sum(uc => uc.AllowUsage) - c.UserCoupons.Sum(uc => uc.RemainUsage))
            })
            .ToList();

        return result;
    }
}
