using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Repository
{
	public interface ITransactionRepository : IGenericRepository<Transaction>
	{
		Task<List<Transaction>> GetTransactionByFilter(
	string Id, string userId, string status, string type, string courseId);

        Task<Transaction> GetByPaymentIntentId(string paymentIntentId);
		Task<List<Transaction>> GetTransactionByUserId(string userId);
		Task<Transaction> CheckTransactionPending(string userId);
		Task<List<Transaction>> CheckTransfer(string transactionId);
	}
}
