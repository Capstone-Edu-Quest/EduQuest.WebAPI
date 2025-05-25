
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Domain.Repository
{
	public interface ILessonContentRepository : IGenericRepository<LessonContent>
	{
		Task<int> GetCurrentContentIndex(string lessonId, int currentIndex);
		Task<List<string>> GetListContentIdByLessonId(string lessonId);
		Task<List<LessonContent>> GetContentsByLessonIdAsync(string lessonId);
		Task<List<LessonContent>> GetByListLessonId(List<string> lessonIds);
		Task<int> GetTotalContent(string courseId);
		//Task<List<LessonContent>> GetLessonMaterialByMaterialId(string materialId);
		Task<bool> IsLessonContentUsed(string contentId);
		Task<TypeOfMaterial?> GetMaterialTypeByIdAsync(string lesonContentId);
	}
}
