using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Subscriptions;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
	{
		private readonly ApplicationDbContext _context;

		public SubscriptionRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<RolePackageNumbersDto> GetPackageNumbersByRole(int roleId)
		{
			var result = await _context.Subscriptions
				.Where(s => s.RoleId == roleId.ToString() &&
						   s.Config.ToLower() != GeneralEnums.ConfigEnum.PriceMonthly.ToString().ToLower() &&
						   s.Config.ToLower() != GeneralEnums.ConfigEnum.PriceYearly.ToString().ToLower())
				.ToListAsync();

			var rolePackageNumbersDto = new RolePackageNumbersDto
			{
				Free = new Dictionary<GeneralEnums.ConfigEnum, decimal?>(),
				Pro = new Dictionary<GeneralEnums.ConfigEnum, decimal?>()
			};

			foreach (var subscription in result)
			{
				// Phân loại dữ liệu theo loại gói Free hoặc Pro
				if (subscription.PackageType.ToLower() == GeneralEnums.PackageEnum.Free.ToString().ToLower())
				{
					// Gán các giá trị vào gói Free
					if (subscription.Config.ToLower() == GeneralEnums.ConfigEnum.CommissionFee.ToString().ToLower())
						rolePackageNumbersDto.Free[GeneralEnums.ConfigEnum.CommissionFee] = subscription.Value;
				}
				else if (subscription.PackageType.ToLower() == GeneralEnums.PackageEnum.Pro.ToString().ToLower())
				{
					// Gán các giá trị vào gói Pro
					if (subscription.Config.ToLower() == GeneralEnums.ConfigEnum.CommissionFee.ToString().ToLower())
						rolePackageNumbersDto.Pro[GeneralEnums.ConfigEnum.CommissionFee] = subscription.Value;
					else if (subscription.Config.ToLower() == GeneralEnums.ConfigEnum.CouponDiscountUpto.ToString().ToLower())
						rolePackageNumbersDto.Pro[GeneralEnums.ConfigEnum.CouponDiscountUpto] = subscription.Value;
					else if (subscription.Config.ToLower() == GeneralEnums.ConfigEnum.MarketingEmailPerMonth.ToString().ToLower())
						rolePackageNumbersDto.Pro[GeneralEnums.ConfigEnum.MarketingEmailPerMonth] = subscription.Value;
					else if (subscription.Config.ToLower() == GeneralEnums.ConfigEnum.CouponPerMonth.ToString().ToLower())
						rolePackageNumbersDto.Pro[GeneralEnums.ConfigEnum.CouponPerMonth] = subscription.Value;
					else if (subscription.Config.ToLower() == GeneralEnums.ConfigEnum.ExtraGoldAndExp.ToString().ToLower())
						rolePackageNumbersDto.Pro[GeneralEnums.ConfigEnum.ExtraGoldAndExp] = subscription.Value;
					else if (subscription.Config.ToLower() == GeneralEnums.ConfigEnum.TrialCoursePercentage.ToString().ToLower())
						rolePackageNumbersDto.Pro[GeneralEnums.ConfigEnum.TrialCoursePercentage] = subscription.Value;
					else if (subscription.Config.ToLower() == GeneralEnums.ConfigEnum.CourseTrialPerMonth.ToString().ToLower())
						rolePackageNumbersDto.Pro[GeneralEnums.ConfigEnum.CourseTrialPerMonth] = subscription.Value;
				}
			}

			return rolePackageNumbersDto;
		}


		public async Task<RolePackageDto> GetPackgaePriceByRole(int roleId)
		{
			var result = await _context.Subscriptions
			.Where(s => s.RoleId == roleId.ToString() &&
					   s.Config.ToLower() == GeneralEnums.ConfigEnum.PriceMonthly.ToString().ToLower() || (s.RoleId == roleId.ToString() &&
						s.Config.ToLower() == GeneralEnums.ConfigEnum.PriceYearly.ToString().ToLower()))
			.ToListAsync();

			var rolePackageDto = new RolePackageDto
			{
				Monthly = 0,
				Yearly = 0
			};

			foreach (var subscription in result)
			{
				if (subscription.Config.ToLower() == GeneralEnums.ConfigEnum.PriceMonthly.ToString().ToLower())
				{
					rolePackageDto.Monthly = subscription.Value;
				}
				else if (subscription.Config.ToLower() == GeneralEnums.ConfigEnum.PriceYearly.ToString().ToLower())
				{
					rolePackageDto.Yearly = subscription.Value;
				}
			}

			return rolePackageDto;
		}
	}
}
