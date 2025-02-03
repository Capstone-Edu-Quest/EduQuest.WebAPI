using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.Repository
{
	public class UserStatisticRepository : GenericRepository<UserStatistic>, IUserStatisticRepository
	{
		private readonly ApplicationDbContext _context;

		public UserStatisticRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<UserStatistic> GetByUserId(string userId)
		{
			return await _context.UserStatistics.FirstOrDefaultAsync(x => x.UserId.Equals(userId));
		}
	}
}
