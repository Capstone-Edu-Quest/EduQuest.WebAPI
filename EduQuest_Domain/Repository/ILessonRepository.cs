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
		Task<List<LessonContent>> GetContentsByLessonId(string lessonId);
		Task<Lesson> GetFirstLesson(string courseId);
		Task<Lesson> GetFirstLessonInCourseAsync(string courseId);
		Task<double> CalculateContentProgressAsync(string lessonId, string contentId, int totalContent);
		Task<double> CalculateAssignmentProgressAsync(string lessonId, string assignmentId, int totalMaterial);
		Task<double> CalculateQuizProgressAsync(string lessonId, string quizId, int totalMaterial);
        Task<Lesson> GetLessonByCourseIdAndIndex(string courseId, int index);
		Task<double> CalculateContentProgressBeforeCurrentAsync(string lessonId, int currentIndex, int totalContent);

	}
}
