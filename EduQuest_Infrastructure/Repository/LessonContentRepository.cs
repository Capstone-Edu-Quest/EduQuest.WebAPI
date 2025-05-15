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
	public class LessonContentRepository : GenericRepository<LessonContent>, ILessonContentRepository
	{
		private readonly ApplicationDbContext _context;

		public LessonContentRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<LessonContent>> GetByListLessonId(List<string> lessonIds)
		{
			return await _context.LessonContents.Where(x => lessonIds.Contains(x.LessonId)).ToListAsync();
		}

		public async Task<int> GetCurrentContentIndex(string lessonId, int currentIndex)
		{
			var entity =  await _context.LessonContents.FirstOrDefaultAsync(x => x.LessonId == lessonId && x.Index == currentIndex);
			return entity != null ? entity.Index : 0;
		}

		//public async Task<List<LessonContent>> GetLessonMaterialByMaterialId(string materialId)
		//{
		//	return await _context.LessonMaterials.Where(x => x.MaterialId == materialId).ToListAsync();
		//}

		public async Task<List<string>> GetListContentIdByLessonId(string lessonId)
		{
			var result = await _context.LessonContents
				.Where(x => x.LessonId == lessonId)
				.OrderBy(x => x.Index)
				.Select(x =>
					x.MaterialId ?? x.QuizId ?? x.AssignmentId)
				.Where(id => id != null)
				.ToListAsync();

			return result!;
		}


		public async Task<List<LessonContent>> GetContentsByLessonIdAsync(string lessonId)
		{
			return await _context.LessonContents
				.Where(x => x.LessonId == lessonId)
				.OrderBy(x => x.Index)
				.ToListAsync();
		}

		public async Task<int> GetTotalContent(string courseId)
		{
			var listLessonId = (await _context.Lessons.Where(x => x.CourseId == courseId).ToListAsync()).Select(x => x.Id);
			var listLessonContent = await _context.LessonContents.Where(x => listLessonId.Contains(x.LessonId)).ToListAsync();
			return listLessonContent.Count;
		}

		public async Task<bool> IsLessonContentUsed(string contentId)
		{
			var isUsed = await _context.LessonContents
				.Where(x => x.MaterialId == contentId || x.QuizId == contentId || x.AssignmentId == contentId)
				.AnyAsync(lm => lm.Lesson.Course.Status == GeneralEnums.StatusCourse.Public.ToString());

			return isUsed;
		}
	}
}
