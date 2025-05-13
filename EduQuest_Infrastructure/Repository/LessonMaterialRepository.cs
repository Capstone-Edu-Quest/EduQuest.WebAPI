using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
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
	public class LessonMaterialRepository : GenericRepository<LessonContent>, ILessonMaterialRepository
	{
		private readonly ApplicationDbContext _context;

		public LessonMaterialRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<LessonContent>> GetByListLessonId(List<string> lessonIds)
		{
			return await _context.LessonMaterials.Where(x => lessonIds.Contains(x.LessonId)).ToListAsync();
		}

		public async Task<int> GetCurrentMaterialIndex(string lessonId, int currentIndex)
		{
			var entity =  await _context.LessonMaterials.FirstOrDefaultAsync(x => x.LessonId == lessonId && x.Index == currentIndex);
			return entity != null ? entity.Index : 0;
		}

		//public async Task<List<LessonContent>> GetLessonMaterialByMaterialId(string materialId)
		//{
		//	return await _context.LessonMaterials.Where(x => x.MaterialId == materialId).ToListAsync();
		//}

		public async Task<List<string>> GetListMaterialIdByLessonId(string lessonId)
		{
			var list = await _context.LessonMaterials
				.Where(x => x.LessonId == lessonId)
				.OrderBy(x => x.Index)
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

		public async Task<int> GetTotalMaterial(string courseId)
		{
			var listLessonId = (await _context.Lessons.Where(x => x.CourseId == courseId).ToListAsync()).Select(x => x.Id);
			var listLessonMaterial = await _context.LessonMaterials.Where(x => listLessonId.Contains(x.LessonId)).ToListAsync();
			return listLessonMaterial.Count;
		}

		public async Task<bool> IsMaterialUsed(string materialId)
		{
			var isUsed = await _context.LessonMaterials
				.Where(x => x.MaterialId == materialId)
				.AnyAsync(lm => lm.Lesson.Course.Status == GeneralEnums.StatusCourse.Public.ToString());

			return isUsed;
		}
	}
}
