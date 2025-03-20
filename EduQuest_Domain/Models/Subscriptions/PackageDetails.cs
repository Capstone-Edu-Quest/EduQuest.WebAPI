using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Models.Subscriptions
{
	public class PackageDetails
	{
		public decimal? CommisionFee { get; set; }
		public decimal? MarketingEmailPerMonth { get; set; }
		public decimal? CouponPerMonth { get; set; }
		public decimal? CouponDiscountUpto { get; set; }
		public decimal? ExtraGoldAndExp { get; set; }
		public decimal? TrialCoursePercentage { get; set; }
		public decimal? CourseTrialPerMonth { get; set; }
	}
}
