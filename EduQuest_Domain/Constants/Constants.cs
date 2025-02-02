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
			public const string GetSuccesfully = "GET_SUCCESSFULLY";
			public const string GetFailed = "GET_FAILED";
			public const string SavingSuccesfully = "SAVING_SUCCESSFULLY";
			public const string SavingFailed = "SAVING_FAILED";
			public const string DeleteSuccessfully = "DELETE_SUCCESFULLY";
			public const string DeleteFailed = "DELETE_EVENT_FAIL";
			public const string UpdateSuccesfully = "UPDATE_SUCCESSFULLY";
			public const string UpdateFailed = "UPDATE_FAILED";
			public const string CreateSuccesfully = "CREATED_SUCCESSFULLY";
			public const string CreateFailed = "CREATED_FAILED";

			public const string NotFound = "NOT_FOUND";
			public const string Complete = "COMPLETE";

			public const string ServerError = "SOMETHINGS WENT WRONG";
			public const string SessionTimeout = "SESSION_TIMEOUT";

			public const string ReturnListHasValue = "LIST_HAS_VALUE";
			public const string lockAcquired = "LOCK_ACQUIRED";

			//public const string Unauthorized = "UNAUTHORIZED";
			//public const string Blocked = "USER_BLOCKED";
			//public const string InvalidToken = "INVALID_TOKEN";
			//public const string TokenExpired = "TOKEN_EXPIRED";
			//public const string TokenRefreshSuccess = "REFRESH_SUCCESSFULLY";

			//public const string LoginFailed = "LOGIN_FAILED";
			//public const string UserDontHavePer = "USER_DONT_HAVE_PERMISSION";
		}

		public static class MessageAchievement
		{
			public const string ExistedUser = "USERS HAVE USED";
		}

		#endregion


	}
}
