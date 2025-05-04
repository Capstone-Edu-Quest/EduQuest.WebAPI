namespace EduQuest_Domain.Constants
{
    public class Constants
	{
        public static class FireBaseUrl
        {
            public const string URL = "https://eduquest-notifications.asia-southeast1.firebasedatabase.app/";
        }

        public static class BaseUrl
        {
            //public const string Base = "https://edu-quest-webui.vercel.app";
            public const string ShopItemUrl = $"/shop-items";
        }
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

		public static class NotificationMessage 
		{
            public const string SUBMIT_COURSE_SUCCESSFULLY = "SUBMIT_COURSE_SUCCESSFULLY";
            public const string PURCHASE_ITEM_SUCCESSFULLY = "PURCHASE_ITEM_SUCCESSFULLY";
            public const string NOT_ENOUGH_GOLD = "NOT_ENOUGH_GOLD";
            public const string YOUR_COURSE_WAS_APPROVED= "YOUR_COURSE_WAS_APPROVED";
            public const string YOUR_COURSE_WAS_REJECTED = "YOUR_COURSE_WAS_REJECTED";
            public const string A_STAFF_HAS_ASSIGN_COURSE_TO_YOU= "A_STAFF_HAS_ASSIGN_COURSE_TO_YOU";
            public const string A_STAFF_HAS_ASSIGN_INSTRUCTOR_TO_YOU= "A_STAFF_HAS_ASSIGN_INSTRUCTOR_TO_YOU";
            public const string CHECK_OUT_SUCCESSFULLY = "CHECK_OUT_SUCCESSFULLY";
            public const string BUY_COURSE_SUCCESSFULLY = "BUY_COURSE_SUCCESSFULLY";
            public const string COMPLETED_COURSE_SUCCESSFULLY = "COMPLETED_COURSE_SUCCESSFULLY";
            public const string BUY_COURSE_FAILED = "BUY_COURSE_FAILED";
            public const string YOUR_MONEY_WAS_TRANSFERED_FROM_THE_PLATFORM = "YOUR_MONEY_WAS_TRANSFERED_FROM_THE_PLATFORM";
            public const string REFUND_SUCCESSFULLY = "REFUND_SUCCESSFULLY";
            public const string REFUND_FAILED = "REFUND_FAILED";
            public const string YOUR_COURSE_WAS_RATED = "YOUR_COURSE_WAS_RATED";
            public const string BECOME_INSTRUCTOR_APPROVED = "BECOME_INSTRUCTOR_APPROVED";
            public const string BECOME_INSTRUCTOR_REJECTED = "BECOME_INSTRUCTOR_REJECTED";
            public const string LEARNING_PATH_OVERDUE = "LEARNING_PATH_OVERDUE";
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
			public const string Expert = "Expert";
			public const string Staff = "Staff";
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
			public const string SubmitSuccessfully = "SUBMIT_SUCCESSFULLY";
			public const string GetSuccesfully = "GET_SUCCESSFULLY";
			public const string GetFailed = "GET_FAILED";
			public const string SavingSuccesfully = "SAVING_SUCCESSFULLY";
			public const string SavingFailed = "SAVING_FAILED";
			public const string DeleteSuccessfully = "DELETE_SUCCESFULLY";
			public const string DeleteFailed = "DELETE_FAILED";
			public const string UpdateSuccesfully = "UPDATE_SUCCESSFULLY";
			public const string UpdateFailed = "UPDATE_FAILED";
			public const string CreateSuccesfully = "CREATED_SUCCESSFULLY";
			public const string CreateFailed = "CREATED_FAILED";
			public const string CancelSuccessfully = "CANCEL_SUCCESSFULLY";

			public const string NotFound = "NOT_FOUND";
			public const string NotEnoughGold = "NOT_ENOUGH_GOLD";
			public const string Complete = "COMPLETE";
			public const string NoProvided = "NO_PROVIDED";
            public const string AlreadyExists = "ALREADY_EXISTS";

            public const string ServerError = "Something when wrong";
			public const string SessionTimeout = "Your session has timeout";

			public const string ReturnListHasValue = "User unauthorized";
			public const string lockAcquired = "You have been blocked";

			public const string Unauthorized = "UNAUTHORIZED";
			public const string Blocked = "USER_BLOCKED";
			public const string InvalidToken = "INVALID_TOKEN";
			public const string TokenExpired = "TOKEN_EXPIRED";
            public const string TokenBlackListed = "TOKEN_BLACKLIST";
            public const string LogOutSuccessfully = "SIGN_OUT_SUCCESSFULLY";
            public const string invalidEmailOrPassword = "INVALID_EMAIL_OR_PASSWORD";
            public const string TokenRefreshSuccess = "REFRESH_SUCCESSFULLY";
            public const string ResetPasswordSuccessfully = "RESET_PASSWORD_SUCCESSFULLY";
            public const string VerifyOtp = "VERIFY_OTP_SUCCESSFULLY";
            public const string VerifyOtpFailed = "VERIFY_OTP_FAILED";
            public const string AssignExpert = "ASSIGN_EXPERT_SUCCESSFULLY";
            public const string EmailNotFound = "EMAIL_NOT_FOUND";
            public const string EmailExisted = "EMAIL_EXISTED";
            public const string WrongPassword = "WRONG_PASSWORD";
            public const string PasswordChanged = "PASSWORD_CHANGED_SUCCESSFULLY";
            public const string InvalidRole = "INVALID_ROLE";
            public const string HaventApplyInstructor = "HAVENT_APPLIED_INSTRUCTOR";

            public const string AlreadyOwnThisItem = "ALREADY_OWN_THIS_ITEM";
            public const string PurchaseItemSuccessfully = "PURCHASE_ITEM_SUCCESSFULLY";

			public const string LoginFailed = "Login failed";
			public const string UserDontHavePer = "Permission denied";

			public const string CourseShouldBePending = "COURSE_SHOULD_BE_PENDING";

			public const string SentOtpSuccessfully = "SENT_OTP_SUCCESSFULLY";
			public const string NotOwner = "YOU ARE NOT OWNER OF THIS MATERIAL";

			public const string NotAttemptRecorded = "NO_ATTEMPT_RECORDED";
			public const string SubmitBecomeInstructorSuccessfully = "SUBMIT_BECOME_INSTRUCTOR_SUCCESSFULLY";
			public const string ApproveSuccessfully = "APPROVED_SUCCESSFULLY";

		}

		public static class MessageAchievement
		{
			public const string ExistedUser = "USERS HAVE USED";
		}

		public static class MessageFirebase
		{
			public const string ExistedUser = "USERS HAVE USED";
		}
		public static class MessageLearner
		{
			public const string AddedUserToCourse = "AddedUserToClass";
			public const string NotLearner = "YouarenotlearnerthisClass";
		}
		public static class MessageQuest
        {
            public const string InvalidQuestTypeOrResetType = "QUESTTYPE_OR_TYPE_INVALID";
        }
        public static class MessageError
		{
			public const string FeedbackMaxLength = "MAX_LENGTH_REACHED";
			public const string RatingLimit = "RATING_LIMIT_ERROR";
			public const string NameIsRequired = "NAME_REQUIRED";
			public const string DescriptionRequired = "DESCRIPTION_REQUIRED";
			public const string ValueRequired = "FIELD_REQUIRED";
            public const string DuplicateCourseIdOrCourseOrder = "DETECT_DUPLICATE_COURSE_ID_AND_COURSE_ORDER";
			public const string EnrolledLPUpdateBlock = "ENROLLED_LEARNING_PATH_UPDATE_PROHIBITION";
            public static int NameMaxLength = 500;
            public static int DescriptionMaxLength = 2500;
			public static int MinimumLearningTimeDaily = 180; //minute = 3 hours

            public const string CouponCodeExist = "COUPON_CODE_EXIST";
			public const string NeedToCompletePayment = "NEED_TO_COMPLETE_PAYMENT";
			public const string CannotUpdateCoupon = "START_TIME_PASSED_ERROR";

			public const string LevelExist = "LEVEL_EXISTED";

			public const string QuestAlreadyClaimed = "QUEST_ALREADY_CLAIMED";

			public const string CourseExist = "ALREADY_IN_COURSE";
		}
		#endregion


	}
}
