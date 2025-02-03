using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository
{
	public class SystemConfigRepository : GenericRepository<SystemConfig>, ISystemConfigRepository
	{
		private readonly ApplicationDbContext _context;

		public SystemConfigRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<SystemConfig> GetByName(string name)
		{
			return await _context.SystemConfigs.FirstOrDefaultAsync(x => x.Name.Equals(name));
		}
	}
}
