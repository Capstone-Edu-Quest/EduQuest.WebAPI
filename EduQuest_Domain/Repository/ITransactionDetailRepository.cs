using EduQuest_Application.DTO.Response.Revenue;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Revenue;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
	public interface ITransactionDetailRepository : IGenericRepository<TransactionDetail>
	{
		Task<List<TransactionDetail>> GetByTransactionId(string transactionId);
		Task<RevenueReportDto> GetRevenueReportAsync(string userId);
		Task<List<InstructorTransferInfo>> GetGroupedInstructorTransfersByTransactionId(string transactionId);
		Task<TransactionDetail> GetByTransactionIdAndCourseId(string transactionId, string courseId);
		Task<(DateTime? CreatedAt, decimal Amount)> GetCourseTransactionInfoAsync(string courseId, string userId);
		Task<decimal> GetTotalRevenueByInstructorId(string instructorId);
	}
}
