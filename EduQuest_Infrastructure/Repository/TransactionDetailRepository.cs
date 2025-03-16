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
	}
}
