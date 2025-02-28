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
	}
}
