
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
	public interface ILessonMaterialRepository : IGenericRepository<LessonContent>
	{
		Task<int> GetCurrentMaterialIndex(string lessonId, int currentIndex);
		Task<List<string>> GetListMaterialIdByLessonId(string lessonId);
		Task<List<Material>> GetMaterialsByLessonIdAsync(string lessonId);
		Task<List<LessonContent>> GetByListLessonId(List<string> lessonIds);
		Task<int> GetTotalMaterial(string courseId);
		//Task<List<LessonContent>> GetLessonMaterialByMaterialId(string materialId);
		Task<bool> IsMaterialUsed(string materialId);
	}
}
