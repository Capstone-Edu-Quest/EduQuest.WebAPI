using Unidecode.NET;

namespace EduQuest_Application.Helper
{
	public static class ContentHelper
	{
		public static string ConvertVietnameseToEnglish(string name)
		{
			return name.Unidecode().ToLower();  
		}

		public static string ReplacePlaceholders(string message, Dictionary<string, string> content)
		{
			foreach (var entry in content)
			{
				message = message.Replace($"{{{{{entry.Key}}}}}", entry.Value);
			}
			return message;
		}
	}
}
