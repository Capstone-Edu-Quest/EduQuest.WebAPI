
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
	public interface ILessonMaterialRepository : IGenericRepository<LessonMaterial>
	{
		Task<int> GetCurrentMaterialIndex(string lessonId, string materialId);
		Task<List<string>> GetListMaterialIdByLessonId(string lessonId);
		Task<List<Material>> GetMaterialsByLessonIdAsync(string lessonId);
		Task<List<LessonMaterial>> GetByListLessonId(List<string> lessonIds);
	}
}
