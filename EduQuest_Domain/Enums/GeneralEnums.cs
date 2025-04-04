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

		public enum TypeOfMaterial
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

		#region Transaction
		public enum TypeTransaction
		{
			CheckoutCart = 1,
			Refund = 2,
			ProAccount = 3
		}

		public enum ItemTypeTransaction
		{
			Course = 1,
			
		}
		#endregion

		#region Coupon
		public enum DiscountType
		{
			FixedAmount = 1,  
			Percentage = 2
		}
		#endregion

		#region Subscription
		public enum PackageEnum
		{
			Free = 1,
			Pro = 2
		}
		public enum PackageName
		{
			APIsPackagePrice = 1,
			APIsPackageNumbers = 2
		}

		public enum ConfigEnum
		{
			PriceMonthly = 1,
			PriceYearly = 2,
			CommissionFee = 3,
			MarketingEmailPerMonth  = 4,
			CouponPerMonth = 5, 
			CouponDiscountUpto = 6,
			ExtraGoldAndExp = 7,
			TrialCoursePercentage = 8,
			CourseTrialPerMonth = 9
		}
        #endregion

        #region ReportType
		public enum ReportStatus
        {
            // 1 = resolved, 2 = pending, 3 = rejected
			Resolved = 1,
			Pending = 2,
			Rejected = 3
        }
        #endregion
    }
}
