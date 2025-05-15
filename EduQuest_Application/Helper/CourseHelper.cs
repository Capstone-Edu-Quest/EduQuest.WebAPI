using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Application.DTO.Response.Materials;
using EduQuest_Domain.Entities;
using System.Text;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.Helper
{
	public static class CourseHelper
	{
		public static string GenerateCacheKeySearchCourse(SearchCourseRequest searchRequest, int pageNo, int eachPage)
		{
			var keyBuilder = new StringBuilder();

			keyBuilder.Append($"searchCourse_Page_{pageNo}_Size_{eachPage}");

			if (!string.IsNullOrEmpty(searchRequest.KeywordName))
				keyBuilder.Append($"_KeywordName_{searchRequest.KeywordName}");

			if (searchRequest.DateFrom.HasValue)
				keyBuilder.Append($"_DateFrom_{searchRequest.DateFrom.Value.ToString("yyyyMMdd")}");

			if (searchRequest.DateTo.HasValue)
				keyBuilder.Append($"_DateTo_{searchRequest.DateTo.Value.ToString("yyyyMMdd")}");

			if (searchRequest.TagListId != null && searchRequest.TagListId.Any())
				keyBuilder.Append($"_TagIds_{string.Join("-", searchRequest.TagListId.Where(tag => tag != null))}");

			if (!string.IsNullOrEmpty(searchRequest.Author))
				keyBuilder.Append($"_Author_{searchRequest.Author}");

			if (searchRequest.Rating.HasValue)
				keyBuilder.Append($"_Rating_{searchRequest.Rating.Value}");

			if (searchRequest.Sort.HasValue)
				keyBuilder.Append($"_Sort_{searchRequest.Sort.Value}");

			return keyBuilder.ToString();
		}

		public static ContentInLessonResponse MapLessonContentToResponse(LessonContent content)
		{
			if (content.MaterialId != null && content.Material != null)
			{
				return new ContentInLessonResponse
				{
					Id = content.MaterialId,
					Type = content.Material.Type,
					Duration = content.Material.Duration,
					Title = content.Material.Title,
					Description = content.Material.Description
				};
			}
			else if (content.QuizId != null && content.Quiz != null)
			{
				return new ContentInLessonResponse
				{
					Id = content.QuizId,
					Type = ((int)TypeOfMaterial.Quiz).ToString(),
					Duration = content.Quiz.TimeLimit,
					Title = content.Quiz.Title,
					Description = content.Quiz.Description
				};
			}
			else if (content.AssignmentId != null && content.Assignment != null)
			{
				return new ContentInLessonResponse
				{
					Id = content.AssignmentId,
					Type = ((int)TypeOfMaterial.Assignment).ToString(),
					Duration = content.Assignment.TimeLimit,
					Title = content.Assignment.Title,
					Description = content.Assignment.Description
				};
			}

			return null;
		}


	}
}
