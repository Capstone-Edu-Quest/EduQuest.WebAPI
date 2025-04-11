using EduQuest_Application.DTO.Response.Revenue;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.Repository
{
	public class TransactionDetailRepository : GenericRepository<TransactionDetail>, ITransactionDetailRepository
	{
		private readonly ApplicationDbContext _context;

		public TransactionDetailRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<TransactionDetail>> GetByTransactionId(string transactionId)
		{
			return await _context.TransactionDetails.Where(x => x.TransactionId.Equals(transactionId)).ToListAsync();
		}

		public async Task<RevenueReportDto> GetRevenueReportAsync(string userId)
		{
			var now = DateTime.UtcNow.ToUniversalTime();
			var yearStart = new DateTime(now.Year, 1, 1);
			var lastYearStart = yearStart.AddYears(-1);
			var lastYearEnd = yearStart.AddDays(-1);

			var monthStart = new DateTime(now.Year, now.Month, 1);
			var lastMonthStart = monthStart.AddMonths(-1);
			var lastMonthEnd = monthStart.AddDays(-1);

			var weekStart = now.AddDays(-7);
			var lastWeekStart = now.AddDays(-14);
			var lastWeekEnd = weekStart;

			var query = _context.TransactionDetails
			.Where(t => t.InstructorId == userId && t.DeletedAt == null);

			// Total Revenue
			var totalThisYear = await query.Where(t => t.CreatedAt >= yearStart).SumAsync(t => t.InstructorShare);
			var totalLastYear = await query.Where(t => t.CreatedAt >= lastYearStart && t.CreatedAt <= lastYearEnd).SumAsync(t => t.InstructorShare);
			var totalChange = CalculatePercentageChange((decimal)totalLastYear, (decimal)totalThisYear);

			// Revenue This Month
			var thisMonth = await query.Where(t => t.UpdatedAt >= monthStart).SumAsync(t => t.InstructorShare);
			var lastMonth = await query.Where(t => t.UpdatedAt >= lastMonthStart && t.UpdatedAt <= lastMonthEnd).SumAsync(t => t.InstructorShare);
			var monthChange = CalculatePercentageChange((decimal)lastMonth, (decimal)thisMonth);

			// Revenue Last 7 Days
			var last7Days = await query.Where(t => t.UpdatedAt >= weekStart).SumAsync(t => t.InstructorShare);
			var prev7Days = await query.Where(t => t.UpdatedAt >= lastWeekStart && t.UpdatedAt < lastWeekEnd).SumAsync(t => t.InstructorShare);
			var weekChange = CalculatePercentageChange((decimal)prev7Days, (decimal)last7Days);

			// Available Balance
			var available = await _context.Transactions
				.Where(t => t.Type == "Revenue" && t.UserId == userId && t.DeletedAt == null)
				.SumAsync(t => t.NetAmount);

			var pending = totalThisYear - available;

			return new RevenueReportDto
			{
				TotalRevenue = (decimal)totalThisYear,
				TotalRevenueChangePercent = (decimal)totalChange,
				RevenueThisMonth = (decimal)thisMonth,
				RevenueThisMonthChangePercent = monthChange,
				Revenue7Days = (decimal)last7Days,
				Revenue7DaysChangePercent = weekChange,
				AvailableBalance = (decimal)available,
				PendingBalance = (decimal)pending
			};
		}

		private decimal CalculatePercentageChange(decimal oldValue, decimal newValue)
		{
			if (oldValue == 0) return newValue > 0 ? 100 : 0;
			return Math.Round((newValue - oldValue) / oldValue * 100, 2);
		}
	}
}
