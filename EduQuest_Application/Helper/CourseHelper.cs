using EduQuest_Application.DTO.Request.Courses;
using System.Text;

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

	}
}
