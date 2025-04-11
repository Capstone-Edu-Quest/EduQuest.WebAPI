using EduQuest_Application.DTO.Response.Revenue;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Repository
{
	public interface ITransactionDetailRepository : IGenericRepository<TransactionDetail>
	{
		Task<List<TransactionDetail>> GetByTransactionId(string transactionId);
		Task<RevenueReportDto> GetRevenueReportAsync(string userId);
	}
}
