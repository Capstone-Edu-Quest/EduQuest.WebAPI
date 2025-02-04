using System.ComponentModel;

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
			Admin = 1,
            Instructor = 2,
            Learner = 3,
            Guest = 4
		}

		#endregion

		#region Course
		public enum SortCourse
		{
			MostReviews = 1,
			NewestCourses = 2,
			HighestRated = 3
		}

		public enum TypeOfLearningMetarial
		{
			Docs = 1,
			Video = 2,
			Quiz = 3
		}
		#endregion

	}
}
