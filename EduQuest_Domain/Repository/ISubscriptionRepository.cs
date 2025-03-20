using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Subscriptions;
using EduQuest_Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Domain.Repository
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
	{
		Task<RolePackageDto> GetPackgaePriceByRole(int roleId);
		Task<Dictionary<string, RolePackageNumbersDto>> GetPackageNumbersByRole(int roleId);
	}
}
