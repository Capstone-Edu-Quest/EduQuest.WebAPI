using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Enums.GeneralEnums;

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
	}
}
