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
	public class LearningMaterialRepository : GenericRepository<Material>, ILearningMaterialRepository
	{
		private readonly ApplicationDbContext _context;

		public LearningMaterialRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Material>> GetMaterialsByIds(List<string> materialIds)
		{
			return await _context.Materials.Where(m => materialIds.Contains(m.Id)).ToListAsync();
		}
	}
}
