using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Repository
{
	public interface ILessonRepository : IGenericRepository<Lesson>
	{
		Task<List<Lesson>> GetByCourseId(string id);
		Task<int?> GetMaxLevelInThisCourse(string id);
		Task<bool> DeleteLessonByCourseId(string courseId);
		Task<Lesson> GetByLessonIdAsync(string lessonId);
		Task<List<LessonMaterial>> GetMaterialsByLessonId(string lessonId);
		Task<Lesson> GetFirstLesson(string courseId);
		Task<(string lessonId, string materialId)> GetFirstLessonAndMaterialIdInCourseAsync(string courseId);
		Task<double> CalculateMaterialProgressAsync(string lessonId, string materialId, int totalMaterial);
		Task<Lesson> GetLessonByCourseIdAndIndex(string courseId, int index);
		Task<double> CalculateMaterialProgressBeforeCurrentAsync(string lessonId, string materialId, int totalMaterial);

	}
}
