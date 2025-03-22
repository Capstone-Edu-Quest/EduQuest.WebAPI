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
	public class MaterialRepository : GenericRepository<Material>, IMaterialRepository
	{
		private readonly ApplicationDbContext _context;

		public MaterialRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Material> GetMataterialQuizAssById(string materialId)
		{
			return await _context.Materials.Include(x => x.Quiz).Include(x => x.Assignment).FirstOrDefaultAsync(x => x.Id == materialId);
		}

		public async Task<List<Material>> GetMaterialsByIds(List<string> materialIds)
		{
			return await _context.Materials.Where(m => materialIds.Contains(m.Id)).ToListAsync();
		}
	}
}
