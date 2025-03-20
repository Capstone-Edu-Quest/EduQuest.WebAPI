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

		public async Task<Dictionary<string, RolePackageNumbersDto>> GetPackageNumbersByRole(int roleId)
		{
			var result = await _context.Subscriptions
		.Where(s => (int)s.RoleId == roleId &&
				   (s.PackageType == GeneralEnums.ConfigEnum.PriceMonthly.ToString() || (int)s.RoleId == roleId &&
					s.PackageType == GeneralEnums.ConfigEnum.PriceYearly.ToString()))
		.ToListAsync();

			var formattedResult = new Dictionary<string, RolePackageNumbersDto>();

			foreach (var subscription in result)
			{
				if (!formattedResult.ContainsKey(subscription.RoleId.ToString()))
				{
					// Khởi tạo đối tượng RolePackageNumbersDto cho mỗi RoleId
					formattedResult[subscription.RoleId.ToString()] = new RolePackageNumbersDto
					{
						Free = new Dictionary<GeneralEnums.ConfigEnum, decimal?>(),
						Pro = new Dictionary<GeneralEnums.ConfigEnum, decimal?>()
					};
				}

				var data = formattedResult[subscription.RoleId.ToString()];

				// Phân loại dữ liệu theo loại gói Free hoặc Pro
				if (subscription.PackageType.ToLower() == GeneralEnums.PackageEnum.Free.ToString().ToLower())
				{
					if (subscription.Config == GeneralEnums.ConfigEnum.CommissionFee.ToString().ToLower())
						data.Free[GeneralEnums.ConfigEnum.CommissionFee] = subscription.Value;
					else if (subscription.Config == GeneralEnums.ConfigEnum.CouponPerMonth.ToString())
						data.Free[GeneralEnums.ConfigEnum.CouponPerMonth] = subscription.Value;
					// Thêm các trường khác nếu cần
				}
				else if (subscription.PackageType.ToLower() == GeneralEnums.PackageEnum.Pro.ToString().ToLower())
				{
					if (subscription.Config == GeneralEnums.ConfigEnum.CommissionFee.ToString().ToLower())
						data.Pro[GeneralEnums.ConfigEnum.CommissionFee] = subscription.Value;
					else if (subscription.Config == GeneralEnums.ConfigEnum.MarketingEmailPerMonth.ToString().ToLower())
						data.Pro[GeneralEnums.ConfigEnum.MarketingEmailPerMonth] = subscription.Value;
					else if (subscription.Config == GeneralEnums.ConfigEnum.CouponDiscountUpto.ToString().ToLower())
						data.Pro[GeneralEnums.ConfigEnum.CouponDiscountUpto] = subscription.Value;
					else if (subscription.Config == GeneralEnums.ConfigEnum.ExtraGoldAndExp.ToString().ToLower())
						data.Pro[GeneralEnums.ConfigEnum.ExtraGoldAndExp] = subscription.Value;
					else if (subscription.Config == GeneralEnums.ConfigEnum.TrialCoursePercentage.ToString().ToLower())
						data.Pro[GeneralEnums.ConfigEnum.TrialCoursePercentage] = subscription.Value;
					else if (subscription.Config == GeneralEnums.ConfigEnum.CourseTrialPerMonth.ToString().ToLower())
						data.Pro[GeneralEnums.ConfigEnum.CourseTrialPerMonth] = subscription.Value;

				}
			}

			return formattedResult;
		}

		public async Task<RolePackageDto> GetPackgaePriceByRole(int roleId)
		{
			var result = await _context.Subscriptions
			.Where(s => (int)s.RoleId == roleId &&
					   (s.PackageType == GeneralEnums.ConfigEnum.PriceMonthly.ToString() || (int)s.RoleId == roleId &&
						s.PackageType == GeneralEnums.ConfigEnum.PriceYearly.ToString()))
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
