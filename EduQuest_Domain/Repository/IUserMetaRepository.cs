using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Repository
{
	public interface IUserMetaRepository : IGenericRepository<UserMeta>
	{
		Task<UserMeta> GetByUserId(string userId);
	}
}
