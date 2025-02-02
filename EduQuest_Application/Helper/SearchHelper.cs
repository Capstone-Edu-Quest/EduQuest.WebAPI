using Unidecode.NET;

namespace EduQuest_Application.Helper
{
	public static class SearchHelper
	{
		public static string ConvertVietnameseToEnglish(string name)
		{
			return name.Unidecode().ToLower();  
		}
	}
}
