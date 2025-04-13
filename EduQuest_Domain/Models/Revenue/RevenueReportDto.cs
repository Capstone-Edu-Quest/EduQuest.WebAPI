using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Revenue
{
	public class RevenueReportDto
	{
		public decimal TotalRevenue { get; set; }
		public decimal TotalRevenueChangePercent { get; set; }

		public decimal RevenueThisMonth { get; set; }
		public decimal RevenueThisMonthChangePercent { get; set; }

		public decimal Revenue7Days { get; set; }
		public decimal Revenue7DaysChangePercent { get; set; }

		public decimal AvailableBalance { get; set; }
		public decimal PendingBalance { get; set; }
	}
}
