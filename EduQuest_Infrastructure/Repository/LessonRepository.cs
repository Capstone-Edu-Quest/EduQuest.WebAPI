using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository
{
	public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
	{
		private readonly ApplicationDbContext _context;

		public LessonRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<bool> DeleteLessonByCourseId(string courseId)
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

		public async Task<double> CalculateMaterialProgressAsync(string lessonId, string materialId, int totalMaterial)
		{
			if (totalMaterial <= 0) return 0;

			// 1. Lấy lesson hiện tại
			var currentLesson = await _context.Lessons
				.AsNoTracking()
				.FirstOrDefaultAsync(l => l.Id == lessonId);

			if (currentLesson == null) return 0;

			var currentLessonIndex = currentLesson.Index;

			// 2. Lấy index của materialId trong lesson
			var targetLessonMaterial = await _context.LessonMaterials
				.AsNoTracking()
				.FirstOrDefaultAsync(lm => lm.LessonId == lessonId && lm.MaterialId == materialId);

			if (targetLessonMaterial == null) return 0;

			var targetMaterialIndex = targetLessonMaterial.Index;

			// 3. Lấy danh sách lessonId trước bài hiện tại
			var lessonIdsBefore = await _context.Lessons
				.AsNoTracking()
				.Where(l => l.CourseId == currentLesson.CourseId && l.Index < currentLessonIndex)
				.Select(l => l.Id)
				.ToListAsync();

			// 4. Đếm số material đã hoàn thành
			var completedMaterialCount = await _context.LessonMaterials
				.AsNoTracking()
				.CountAsync(lm =>
					lessonIdsBefore.Contains(lm.LessonId) ||
					(lm.LessonId == lessonId && lm.Index <= targetMaterialIndex)
				);

			// 5. Trả về tỉ lệ hoàn thành
			return (double)completedMaterialCount / totalMaterial;
		}
        public async Task<double> CalculateAssignmentProgressAsync(string lessonId, string assignmentId, int totalMaterial)
        {
            if (totalMaterial <= 0) return 0;

            // 1. Lấy lesson hiện tại
            var currentLesson = await _context.Lessons
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == lessonId);

            if (currentLesson == null) return 0;

            var currentLessonIndex = currentLesson.Index;

            // 2. Lấy index của materialId trong lesson
            var targetLessonMaterial = await _context.LessonMaterials
                .AsNoTracking()
                .FirstOrDefaultAsync(lm => lm.LessonId == lessonId && lm.AssignmentId == assignmentId);

            if (targetLessonMaterial == null) return 0;

            var targetMaterialIndex = targetLessonMaterial.Index;

            // 3. Lấy danh sách lessonId trước bài hiện tại
            var lessonIdsBefore = await _context.Lessons
                .AsNoTracking()
                .Where(l => l.CourseId == currentLesson.CourseId && l.Index < currentLessonIndex)
                .Select(l => l.Id)
                .ToListAsync();

            // 4. Đếm số material đã hoàn thành
            var completedMaterialCount = await _context.LessonMaterials
                .AsNoTracking()
                .CountAsync(lm =>
                    lessonIdsBefore.Contains(lm.LessonId) ||
                    (lm.LessonId == lessonId && lm.Index <= targetMaterialIndex)
                );

            // 5. Trả về tỉ lệ hoàn thành
            return (double)completedMaterialCount / totalMaterial;
        }
        public async Task<double> CalculateQuizProgressAsync(string lessonId, string quizId, int totalMaterial)
        {
            if (totalMaterial <= 0) return 0;

            // 1. Lấy lesson hiện tại
            var currentLesson = await _context.Lessons
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == lessonId);

            if (currentLesson == null) return 0;

            var currentLessonIndex = currentLesson.Index;

            // 2. Lấy index của materialId trong lesson
            var targetLessonMaterial = await _context.LessonMaterials
                .AsNoTracking()
                .FirstOrDefaultAsync(lm => lm.LessonId == lessonId && lm.AssignmentId == quizId);

            if (targetLessonMaterial == null) return 0;

            var targetMaterialIndex = targetLessonMaterial.Index;

            // 3. Lấy danh sách lessonId trước bài hiện tại
            var lessonIdsBefore = await _context.Lessons
                .AsNoTracking()
                .Where(l => l.CourseId == currentLesson.CourseId && l.Index < currentLessonIndex)
                .Select(l => l.Id)
                .ToListAsync();

            // 4. Đếm số material đã hoàn thành
            var completedMaterialCount = await _context.LessonMaterials
                .AsNoTracking()
                .CountAsync(lm =>
                    lessonIdsBefore.Contains(lm.LessonId) ||
                    (lm.LessonId == lessonId && lm.Index <= targetMaterialIndex)
                );

            // 5. Trả về tỉ lệ hoàn thành
            return (double)completedMaterialCount / totalMaterial;
        }
        public async Task<Lesson> GetLessonByCourseIdAndIndex(string courseId, int index)
		{
			return await _context.Lessons.FirstOrDefaultAsync(l => l.CourseId == courseId && l.Index == index);
		}

		public async Task<double> CalculateMaterialProgressBeforeCurrentAsync(string lessonId, int currentIndex, int totalMaterial)
		{
			if (totalMaterial <= 0) return 0;

			// 1. Lấy bài học hiện tại
			var currentLesson = await _context.Lessons
				.AsNoTracking()
				.FirstOrDefaultAsync(l => l.Id == lessonId);

			if (currentLesson == null) return 0;

			var currentLessonIndex = currentLesson.Index;

			// 2. Lấy index của material hiện tại
			var targetMaterial = await _context.LessonMaterials
				.AsNoTracking()
				.FirstOrDefaultAsync(lm => lm.LessonId == lessonId && lm.Index == currentIndex);

			if (targetMaterial == null) return 0;

			var targetMaterialIndex = targetMaterial.Index;

			// 3. Lấy tất cả lesson trước bài hiện tại
			var lessonIdsBefore = await _context.Lessons
				.AsNoTracking()
				.Where(l => l.CourseId == currentLesson.CourseId && l.Index < currentLessonIndex)
				.Select(l => l.Id)
				.ToListAsync();

			// 4. Đếm toàn bộ material của các bài học trước
			var countBeforeLesson = await _context.LessonMaterials
				.AsNoTracking()
				.CountAsync(lm => lessonIdsBefore.Contains(lm.LessonId));

			// 5. Cộng thêm các material trong chính bài hiện tại nhưng có index < material hiện tại
			var countBeforeInCurrentLesson = await _context.LessonMaterials
				.AsNoTracking()
				.CountAsync(lm => lm.LessonId == lessonId && lm.Index < targetMaterialIndex);

			var completedMaterial = countBeforeLesson + countBeforeInCurrentLesson;

			// 6. Tính tỷ lệ
			return (double)completedMaterial / totalMaterial;
		}
	}
}
