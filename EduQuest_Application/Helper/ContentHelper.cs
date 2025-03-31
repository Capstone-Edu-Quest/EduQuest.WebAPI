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

		public static string JoinStrings(List<string> list, char separator)
		{
			return string.Join(separator, list);
		}

		public static List<string> SplitString(string input, char separator)
		{
			return string.IsNullOrEmpty(input) ? new List<string>() : input.Split(separator).ToList();
		}


	}
}
