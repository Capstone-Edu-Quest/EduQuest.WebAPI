using EduQuest_Application.DTO.Response.Revenue;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.CourseStatistics;
using EduQuest_Domain.Models.Revenue;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Enums.GeneralEnums;

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

		public async Task<TransactionDetail> GetByTransactionIdAndCourseId(string transactionId, string courseId)
		{
			return await _context.TransactionDetails.FirstOrDefaultAsync(x => x.TransactionId == transactionId && x.ItemId == courseId);
		}

		public async Task<(DateTime? CreatedAt, decimal Amount)> GetCourseTransactionInfoAsync(string courseId, string userId)
		{
			var result = await(from td in _context.TransactionDetails
							   join t in _context.Transactions
								   on td.TransactionId equals t.Id
							   where td.ItemId == courseId &&
									 td.ItemType == GeneralEnums.ItemTypeTransactionDetail.Course.ToString() &&
									 t.Type == GeneralEnums.TypeTransaction.CheckoutCart.ToString() &&
									 t.Status == GeneralEnums.StatusPayment.Completed.ToString() &&
									 t.UserId == userId
							   orderby td.CreatedAt descending // Nếu có nhiều, lấy gần nhất
							   select new
							   {
								   CreatedAt = td.CreatedAt,
								   Amount = td.Amount
							   })
					   .FirstOrDefaultAsync();
			return (result.CreatedAt, result.Amount);
		}

		public async Task<List<CourseRevenue>> GetCourseRevenue(List<string> courseIds)
		{
			var courseQuery = await (from course in _context.Courses
									 where courseIds.Contains(course.Id)
									 select new
									 {
										 course.Id,
										 CourseName = course.Title
									 }).ToListAsync();

			var transactionDetails = await _context.TransactionDetails
				.Where(td => courseIds.Contains(td.ItemId) && td.ItemType == ItemTypeTransactionDetail.Course.ToString())
				.ToListAsync();

			var transactionIds = transactionDetails
				.Select(td => td.TransactionId)
				.Distinct()
				.ToList();

			var refundTransactions = await _context.Transactions
			.Where(t => t.Type == TypeTransaction.Refund.ToString() && t.BaseTransactionId != null && transactionIds.Contains(t.BaseTransactionId))
			.ToListAsync();
			var grouped = transactionDetails
			.GroupBy(td => td.ItemId)
			.ToDictionary(g => g.Key, g => new
			{
				TotalSales = g.Count(),
				TotalRevenue = g.Sum(td => td.InstructorShare)
			});
				var refundGrouped = refundTransactions
			.GroupBy(t => t.BaseTransactionId)
			.Select(g => g.Key)
			.ToHashSet();

			var result = courseQuery.Select(c => new CourseRevenue
			{
				Title = c.CourseName,
				TotalSales = grouped.ContainsKey(c.Id) ? grouped[c.Id].TotalSales : 0,
				TotalRevenue = grouped.ContainsKey(c.Id) ? grouped[c.Id].TotalRevenue : 0,
				TotalRefund = transactionDetails
					.Where(td => td.ItemId == c.Id && td.TransactionId != null && refundGrouped.Contains(td.TransactionId))
					.Select(td => td.TransactionId)
					.Distinct()
					.Count()
			}).ToList();

			return result;
		}

		public async Task<List<InstructorTransferInfo>> GetGroupedInstructorTransfersByTransactionId(string transactionId)
		{
			return await _context.TransactionDetails
			.Where(d => d.TransactionId == transactionId && d.InstructorId != null && d.InstructorShare != null)
			.GroupBy(d => d.InstructorId)
			.Select(g => new InstructorTransferInfo
			{
				InstructorId = g.Key,
				TotalInstructorShare = g.Sum(x => x.InstructorShare.Value),
				TransferGroup = $"ORDER_{transactionId}_INSTR_{g.Key}"
			})
			.ToListAsync();
		}

		public async Task<RevenueReportDto> GetRevenueReportAsync(string userId, string email)
		{
			var now = DateTime.Now.ToUniversalTime();
			var yearStart = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var lastYearStart = yearStart.AddYears(-1);
			var lastYearEnd = yearStart.AddDays(-1);

			var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
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
				.Where(t => t.Type == "Transfer" && t.CustomerEmail == email && t.DeletedAt == null)
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

		public async Task<decimal> GetTotalRevenueByInstructorId(string instructorId)
		{
			var now = DateTime.Now.ToUniversalTime();
			var yearStart = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var query = _context.TransactionDetails
			.Where(t => t.InstructorId == instructorId && t.DeletedAt == null);

			// Total Revenue
			var totalThisYear = await query.Where(t => t.CreatedAt >= yearStart).SumAsync(t => t.InstructorShare);
			return (decimal)totalThisYear;
		}

		private decimal CalculatePercentageChange(decimal oldValue, decimal newValue)
		{
			if (oldValue == 0) return newValue > 0 ? 100 : 0;
			return Math.Round((newValue - oldValue) / oldValue * 100, 2);
		}

		public async Task<(List<ChartInfo> Earnings, List<ChartInfo> Sales, List<ChartInfo> Refunds)> GetChartRevenue(string instructorId)
		{
			// Lấy toàn bộ dữ liệu TransactionDetail theo InstructorId
			var details = await _context.TransactionDetails
				.Where(t => t.InstructorId == instructorId && t.ItemType == ItemTypeTransactionDetail.Course.ToString() && t.DeletedAt == null)
				.ToListAsync();

			// Earnings
			var earnings = details
				.Where(x => x.CreatedAt.HasValue)
				.GroupBy(t => new { t.CreatedAt.Value.Year, t.CreatedAt.Value.Month })
				.Select(g => new ChartInfo
				{
					Time = $"{DateTimeHelper.GetMonthName(g.Key.Month)} {g.Key.Year}",
					Count = g.Sum(x => x.InstructorShare ?? 0).ToString("0.##")
				})
				.OrderBy(x => DateTime.ParseExact(x.Time, "MMMM yyyy", CultureInfo.InvariantCulture))
				.ToList();

			// Sales
			var sales = details
				.Where(x => x.CreatedAt.HasValue)
				.GroupBy(t => new { t.CreatedAt.Value.Year, t.CreatedAt.Value.Month })
				.Select(g => new ChartInfo
				{
					Time = $"{DateTimeHelper.GetMonthName(g.Key.Month)} {g.Key.Year}",
					Count = g.Sum(x => x.Amount).ToString("0.##")
				})
				.OrderBy(x => DateTime.ParseExact(x.Time, "MMMM yyyy", CultureInfo.InvariantCulture))
				.ToList();

			// Refunds: Lấy tất cả transactionId từ transactionDetail
			var transactionIds = details
				.Select(x => x.TransactionId)
				.Where(id => !string.IsNullOrEmpty(id))
				.Distinct()
				.ToList();

			var refunds = await _context.Transactions
				.Where(t => t.Type == "Refund"
					&& t.DeletedAt == null
					&& t.BaseTransactionId != null
					&& transactionIds.Contains(t.BaseTransactionId))
				.ToListAsync();

			var refundGroup = refunds
				.Where(x => x.CreatedAt.HasValue)
				.GroupBy(t => new { t.CreatedAt.Value.Year, t.CreatedAt.Value.Month })
				.Select(g => new ChartInfo
				{
					Time = $"{DateTimeHelper.GetMonthName(g.Key.Month)} {g.Key.Year}",
					Count = g.Sum(t => t.TotalAmount).ToString("0.##")
				})
				.OrderBy(x => DateTime.ParseExact(x.Time, "MMMM yyyy", CultureInfo.InvariantCulture))
				.ToList();

			return (earnings, sales, refundGroup);
		}
	}
}
