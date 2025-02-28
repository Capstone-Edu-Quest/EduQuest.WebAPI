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
        return await _context.Coupon.AnyAsync(x => x.Code == code);
    }

    public async Task<PagedList<Coupon>> GetAllByCourseId(string courseId, int pageNo, int pageSize, double? discountValue, string? couponCode, DateTime? expiredAt)
    {
        var result = _context.Coupon.Include(c => c.User).Where(c => c.CourseId == courseId);

        if(discountValue.HasValue)
        {
            result = from r in result
                     where r.DiscountValue >= (decimal)discountValue.Value
                     select r;
        }
        if(expiredAt.HasValue)
        {
            result = from r in result
                     where r.ExpireAt >= expiredAt.Value
                     select r;
        }
        if(!string.IsNullOrWhiteSpace(couponCode))
        {
            result = from r in result
                     where r.Code.Contains(couponCode!)
                     select r;
        }
        return await result.Pagination(pageNo, pageSize).ToPagedListAsync(pageNo, pageSize);
    }

    public async Task<PagedList<Coupon>> GetAllPlatformCoupon(int pageNo, int pageSize, double? discountValue, string? couponCode, DateTime? expiredAt)
    {
        var result = _context.Coupon.Include(c => c.User).Where(c => c.CourseId == null);

        if (discountValue.HasValue)
        {
            result = from r in result
                     where r.DiscountValue >= (decimal)discountValue.Value
                     select r;
        }
        if (expiredAt.HasValue)
        {
            result = from r in result
                     where r.ExpireAt >= expiredAt.Value
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
}
