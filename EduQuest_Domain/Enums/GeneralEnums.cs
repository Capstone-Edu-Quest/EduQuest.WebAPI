using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Enums
{
	public class GeneralEnums
	{
		#region User
		public enum AccountStatus
		{
			[Description("All")]
			All = 0,
			[Description("Active")]
			Active = 1,
			[Description("Pending")]
			Pending = 2,
			[Description("Blocked")]
			Blocked = 3
		}

		public enum UserRole
		{
			Guest = 1,
			User = 2,
			Admin = 3
		}

		#endregion

		#region Course
		public enum SortCourse
		{
			MostReviews = 1,
			NewestCourses = 2,
			HighestRated = 3
		}
		#endregion

	}
}
