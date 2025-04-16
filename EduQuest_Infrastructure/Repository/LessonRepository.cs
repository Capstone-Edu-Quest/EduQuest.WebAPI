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
	public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
	{
		private readonly ApplicationDbContext _context;

		public LessonRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<bool> DeleteStagesByCourseId(string courseId)
		{
			var stages = _context.Lessons.Where(x => x.CourseId == courseId).ToList();

			if(stages.Any())
			{
				_context.Lessons.RemoveRange(stages);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<List<Lesson>> GetByCourseId(string id)
		{
			return await _context.Lessons.Where(x => x.CourseId.Equals(id)).ToListAsync();
		}

		public async Task<Lesson> GetByLessonIdAsync(string lessonId)
		{
			return await _context.Lessons.Include(x => x.LessonMaterials).FirstOrDefaultAsync(x => x.Id == lessonId);
		}

		public async Task<int?> GetMaxLevelInThisCourse(string id)
		{
			return await _context.Lessons.MaxAsync(s => (int?)s.Index);
		}

		public async Task<List<LessonMaterial>> GetMaterialsByLessonId(string lessonId)
		{
			var temp = await _context.Lessons.Where(l => l.Id == lessonId).FirstOrDefaultAsync();
			List<LessonMaterial> result = temp!.LessonMaterials.OrderBy(x => x.Index).ToList();
			return result;
		}

		public async Task<Lesson> GetFirstLesson(string courseId)
		{
			return await _context.Lessons.Include(x => x.LessonMaterials).FirstOrDefaultAsync(x => x.CourseId == courseId && x.Index == 1);
		}

		public async Task<(string lessonId, string materialId)> GetFirstLessonAndMaterialIdInCourseAsync(string courseId)
		{
			var firstLesson = await _context.Lessons
			.Where(lesson => lesson.CourseId == courseId) 
			.OrderBy(lesson => lesson.Index) 
			.FirstOrDefaultAsync();

			if (firstLesson == null)
			{
				return (null, null);
			}

			var firstMaterialId = await _context.LessonMaterials
				.Where(lm => lm.LessonId == firstLesson.Id)
				.OrderBy(lm => lm.Index) 
				.Select(lm => lm.MaterialId)
				.FirstOrDefaultAsync();

			return (firstLesson.Id, firstMaterialId);
		}

		public async Task<decimal> CalculateMaterialProgressAsync(string lessonId, string materialId, decimal courseTotalTime)
		{
			var currentLesson = await _context.Lessons
			.AsNoTracking()
			.FirstOrDefaultAsync(l => l.Id == lessonId);


			var courseId = currentLesson.CourseId;
			var currentLessonIndex = currentLesson.Index;

			// 2. Lấy index của materialId trong lessonId
			var targetLessonMaterial = await _context.LessonMaterials
				.AsNoTracking()
				.FirstOrDefaultAsync(lm => lm.LessonId == lessonId && lm.MaterialId == materialId);


			var targetMaterialIndex = targetLessonMaterial.Index;

			// 3. Lấy tất cả lessonIds có index < hoặc = currentLesson
			var relevantLessons = await _context.Lessons
				.AsNoTracking()
				.Where(l => l.CourseId == courseId && l.Index <= currentLessonIndex)
				.Select(l => new { l.Id, l.Index })
				.ToListAsync();

			// 4. Lấy tất cả material theo logic:
			var allRelevantMaterials = await _context.LessonMaterials
				.AsNoTracking()
				.Include(lm => lm.Material)
				.Where(lm =>
					// Với lesson trước bài hiện tại → lấy tất cả material
					relevantLessons.Any(rl => rl.Id == lm.LessonId && rl.Index < currentLessonIndex)
					||
					// Với lesson hiện tại → chỉ lấy material index <= material hiện tại
					(lm.LessonId == lessonId && lm.Index <= targetMaterialIndex)
				)
				.ToListAsync();

			// 5. Tính tổng duration
			var totalDuration = allRelevantMaterials.Sum(lm => lm.Material?.Duration ?? 0);

			// 6. Tính tỷ lệ so với courseTotalTime
			return courseTotalTime > 0 ? (decimal)totalDuration / courseTotalTime : 0;
		}
	}
}
