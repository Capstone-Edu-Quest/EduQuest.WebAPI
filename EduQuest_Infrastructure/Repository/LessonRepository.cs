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
	}
}
