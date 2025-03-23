using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Subscriptions;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
	public interface ISubscriptionRepository : IGenericRepository<Subscription>
	{
		Task<RolePackageDto> GetPackgaePriceByRole(int roleId);
		Task<RolePackageNumbersDto> GetPackageNumbersByRole(int roleId);
		Task<Subscription> GetSubscriptionByRoleIPackageConfig(string roleId, int packageEnum, int configEnum);
	}
}
