using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
