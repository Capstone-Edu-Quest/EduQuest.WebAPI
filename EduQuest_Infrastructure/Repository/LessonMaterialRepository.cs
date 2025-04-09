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
			return entity != null ? entity.Index : 0;
		}

		public async Task<List<string>> GetListMaterialIdByLessonId(string lessonId)
		{
			var list = await (from lessonMaterial in _context.LessonMaterials
							  join lesson in _context.Lessons on lessonMaterial.LessonId equals lesson.Id
							  where lessonMaterial.LessonId == lessonId
							  orderby lesson.Index, lessonMaterial.Index // Sắp xếp theo Index của Lesson trước, sau đó mới tới Index của Material
							  select lessonMaterial)
					 .ToListAsync();

			return list.Select(x => x.MaterialId).ToList();
		}

		public async Task<List<Material>> GetMaterialsByLessonIdAsync(string lessonId)
		{
			var materials = await _context.LessonMaterials
				.Where(lm => lm.LessonId == lessonId)
				.OrderBy(lm => lm.Lesson.Index)
				.ThenBy(lm => lm.Index)
				.Select(lm => lm.Material)
				.ToListAsync();

			return materials;
		}


	}
}
