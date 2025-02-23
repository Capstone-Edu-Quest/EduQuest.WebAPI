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
            Guest = 4,
			Expert = 5,
			Staff = 6
		}

		#endregion

		#region Course
		public enum SortCourse
		{
			MostReviews = 1,
			NewestCourses = 2,
			HighestRated = 3
		}

		public enum StatusCourse
		{
			Draft = 1,
			Pending = 2,
			Public = 3
		}

		public enum TypeOfLearningMetarial
		{
			Video = 1,
			Document = 2,
			Quiz = 3,
			Assignment = 4
		}
		#endregion

		#region Payment
		public enum StatusPayment
		{
			Pending = 1,
			Completed = 2,
			Failed = 3,
			Expired = 4,
			Canceled = 5
			
		}
		#endregion

	}
}
