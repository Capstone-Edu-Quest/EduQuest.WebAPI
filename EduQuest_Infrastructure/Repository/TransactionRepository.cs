using EduQuest_Application.DTO.Request.Revenue;
using EduQuest_Application.DTO.Response.Revenue;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using static EduQuest_Domain.Enums.GeneralEnums;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace EduQuest_Infrastructure.Repository
{
	public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
	{
		private readonly ApplicationDbContext _context;

		public TransactionRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Transaction> GetByPaymentIntentId(string paymentIntentId)
		{
			return await _context.Transactions.FirstOrDefaultAsync(x => x.PaymentIntentId == paymentIntentId);
		}

		public async Task<List<Transaction>> GetTransactionByUserId(string userId)
		{
			return await _context.Transactions.Where(x => x.UserId == userId).ToListAsync();
		}

        public async Task<List<Transaction>> GetTransactionByFilter(
    string Id, string userId, string status, string type, string courseId)
        {
            var queries = _context.Transactions
                .Include(t => t.TransactionDetails) 
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(Id))
            {
                queries = queries.Where(a => a.Id.Equals(Id));
            }

            if (!string.IsNullOrEmpty(userId))
            {
                queries = queries.Where(a => a.UserId.Equals(userId));
            }

            if (!string.IsNullOrEmpty(status))
            {
                queries = queries.Where(a => a.Status.ToLower().Equals(status.ToLower()));
            }

            if (!string.IsNullOrEmpty(type))
            {
                queries = queries.Where(a => a.Type.ToLower().Equals(type.ToLower()));
            }

            if (!string.IsNullOrEmpty(courseId))
            {
                queries = queries.Where(t =>
                    t.TransactionDetails.Any(d =>
                        d.ItemType == "Course" && d.ItemId == courseId));
            }

            return await queries.ToListAsync();
        }

		public async Task<Transaction> CheckTransactionPending(string userId)
		{
            return await _context.Transactions.FirstOrDefaultAsync(x => x.UserId == userId && x.Type == TypeTransaction.CheckoutCart.ToString() && x.Status == StatusPayment.Pending.ToString());
		}

		public async Task<List<Transaction>> CheckTransfer(string transactionId)
		{
			return await _context.Transactions.Where(x => x.Type == GeneralEnums.TypeTransaction.Transfer.ToString() && x.BaseTransactionId  == transactionId).ToListAsync();   
		}

		public async Task<List<RevenueTransactionResponseForAdmin>> GetRevenueForAdminAsync(RevenueTransactionForAdmin request)
		{
			var baseQuery = _context.Transactions
						.Where(t => t.Status == GeneralEnums.StatusPayment.Completed.ToString());

			// Filter by TransactionType
			if (request.TransactionType == 1)
			{
				baseQuery = baseQuery.Where(t => t.Type == GeneralEnums.TypeTransaction.CheckoutCart.ToString());
			}
			else if (request.TransactionType == 2)
			{
				baseQuery = baseQuery.Where(t => t.Type.ToLower() == ConfigEnum.PriceMonthly.ToString().ToLower() || t.Type.ToLower() == ConfigEnum.PriceYearly.ToString().ToLower());
			}

			// 3. Lấy danh sách transaction đủ điều kiện
			var baseTransactions = await baseQuery.ToListAsync();
			var transactionIds = baseTransactions.Select(t => t.Id).ToList();

			// 4. Truy vấn TransactionDetail
			var detailQuery = _context.TransactionDetails
				.Where(td => transactionIds.Contains(td.TransactionId));

			// 5. Lọc theo UpdatedAt trong TransactionDetail
			if (request.DateFrom.HasValue)
			{
				detailQuery = detailQuery.Where(td => td.UpdatedAt >= request.DateFrom.Value);
			}

			if (request.DateTo.HasValue)
			{
				detailQuery = detailQuery.Where(td => td.UpdatedAt <= request.DateTo.Value);
			}

			var details = await detailQuery.ToListAsync();
			var instructorIds = details
					.Where(d => d.InstructorId != null)
					.Select(d => d.InstructorId)
					.Distinct()
					.ToList();

			var instructorList = await _context.Users
				.Where(u => instructorIds.Contains(u.Id))
				.ToListAsync(); 

			if (!string.IsNullOrWhiteSpace(request.InstructorName))
			{
				var instructorName = ContentHelper.ConvertVietnameseToEnglish(request.InstructorName.ToLower());

				instructorList = instructorList
					.Where(u => ContentHelper.ConvertVietnameseToEnglish(u.Username.ToLower()).Contains(instructorName))
					.ToList();
			}

			var instructors = instructorList.ToDictionary(u => u.Id, u => u.Username);

			// Nếu có InstructorName trong request, chỉ lấy details của instructor đó
			if (!string.IsNullOrWhiteSpace(request.InstructorName))
			{
				details = details
					.Where(d => d.InstructorId != null && instructors.ContainsKey(d.InstructorId))
					.ToList();
			}

			// 6. Kết hợp Transaction với TransactionDetail để trả DTO
			var result = details.Select(detail =>
			{
				var transaction = baseTransactions.First(t => t.Id == detail.TransactionId);
				var isTransferred = _context.Transactions.Any(x =>
					x.Type == GeneralEnums.TypeTransaction.Transfer.ToString() &&
					x.BaseTransactionId == transaction.Id);

				var instructorName = (detail.InstructorId != null && instructors.ContainsKey(detail.InstructorId))
							? instructors[detail.InstructorId]
							: null;
				var isRefund = (detail.NetAmount != 0 && detail.StripeFee < detail.Amount && detail.SystemShare == 0 && detail.InstructorShare == 0) ? true : false;
				return new RevenueTransactionResponseForAdmin
				{
					Id = detail.Id.ToString(),
					TransactionId = transaction.Id,
					Type = transaction.Type == TypeTransaction.CheckoutCart.ToString() ? TypeRevenueForAdmin.Course.ToString() : TypeRevenueForAdmin.Package.ToString(),
					UpdatedAt = detail.UpdatedAt,
					Amount = detail.Amount,
					StripeFee = detail.StripeFee,
					NetAmount = detail.NetAmount,
					SystemShare = detail.SystemShare,
					InstructorShare = detail.InstructorShare,
					InstructorName = instructorName,
					LearnerName = transaction.User.Username,
					IsReceive = isTransferred,
					IsRefund = isRefund,
				};
			}).ToList();

			return result.OrderByDescending(x => x.UpdatedAt).ToList();

		}
	}
}
