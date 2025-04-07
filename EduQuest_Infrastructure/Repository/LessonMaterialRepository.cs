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
	public class LessonMaterialRepository : GenericRepository<LessonMaterial>, ILessonMaterialRepository
	{
		private readonly ApplicationDbContext _context;

		public LessonMaterialRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<int> GetCurrentMaterialIndex(string lessonId, string materialId)
		{
			var entity =  await _context.LessonMaterials.FirstOrDefaultAsync(x => x.LessonId == lessonId && x.MaterialId == materialId);
			return entity.Index;
		}
	}
}
