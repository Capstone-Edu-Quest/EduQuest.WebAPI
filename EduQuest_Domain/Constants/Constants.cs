using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Constants
{
	public class Constants
	{
		public static class Http
		{
			public const string API_VERSION = "v1";
			public const string CORS = "CORS";
			public const string JSON_CONTENT_TYPE = "application/json";
			public const string USER_POLICY = "User";
		}

		public static class HttpContext
		{
			public const string UID = "uid";
			public const string SID = "sid";
			public const string SCP = "http://schemas.microsoft.com/identity/claims/scope";
		}


		#region Authenticate
		public static class PolicyType
		{
			
			public const string Instructor = "Instructor";
			// Just admin system
			public const string Admin = "Admin";
			// Just guest
			public const string Guest = "Guest";
			public const string Learner = "Learner";
		}

		public static class UserClaimType
		{
			public const string UserId = "UserId";
			public const string Role = "Role";
			public const string Status = "Status";
			public const string Avatar = "Avatar";
			public const string Email = "Email";
			public const string AccessToken = "AccessToken";

		}
		#endregion

		#region Entities
		public static class Entities
		{
			public const string USER = "User ";
			public const string ROLE = "Role ";
		}
		#endregion

		#region Message
		public static class MessageCommon
		{
			public const string GetSuccesfully = "List of {{name}}";
			public const string GetFailed = "Failed to get {{name}}";
			public const string SavingSuccesfully = "Saved {{name}} successfully";
			public const string SavingFailed = "Saved {{name}} failed";
			public const string DeleteSuccessfully = "Deleted {{name}} successfully";
			public const string DeleteFailed = "Deleted {{name} failed";
			public const string UpdateSuccesfully = "Updated {{name}} successfully";
			public const string UpdateFailed = "Update {{name}} failed";
			public const string CreateSuccesfully = "Created {{name}} successfully";
			public const string CreateFailed = "Created {{name}} failed";

			public const string NotFound = "{{name}} not found";
			public const string Complete = "Completed";
			public const string NoProvided = "No {{name}} provided";

			public const string ServerError = "Something when wrong";
			public const string SessionTimeout = "Your session has timeout";

			public const string ReturnListHasValue = "User unauthorized";
			public const string lockAcquired = "You have been blocked";

			public const string Unauthorized = "UNAUTHORIZED";
			public const string Blocked = "USER_BLOCKED";
			public const string InvalidToken = "Token invalid";
			public const string TokenExpired = "Token expired";
			public const string TokenRefreshSuccess = "REFRESH_SUCCESSFULLY";

			public const string LoginFailed = "Login failed";
			public const string UserDontHavePer = "Permission denied";

		}

		public static class MessageAchievement
		{
			public const string ExistedUser = "USERS HAVE USED";
		}

		public static class MessageError
		{
			public const string FeedbackMaxLength = "MAX_LENGTH_REACHED";
			public const string RatingLimit = "RATING_LIMIT_ERROR";
			public const string NameIsRequired = "NAME_REQUIRED";
			public const string DescriptionRequired = "DESCRIPTION_REQUIRED";
			public const string ValueRequired = "FIELD_REQUIRED";
            public const string DuplicateCourseIdOrCourseOrder = "DETECT_DUPLICATE_COURSE_ID_AND_COURSE_ORDER";
            public static int NameMaxLength = 500;
            public static int DescriptionMaxLength = 2500;
        }
		#endregion


	}
}
