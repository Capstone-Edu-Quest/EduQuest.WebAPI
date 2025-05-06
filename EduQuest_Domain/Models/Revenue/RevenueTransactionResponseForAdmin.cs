namespace EduQuest_Application.DTO.Response.Revenue
{
	public class RevenueTransactionResponseForAdmin
	{
		public string Id { get; set; }
		public string TransactionId { get; set; }
		public string Type { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public decimal Amount { get; set; }
		public decimal? StripeFee { get; set; }
		public decimal? NetAmount { get; set; }
		public decimal? SystemShare { get; set; }
		public decimal? InstructorShare { get; set; }
		public string? InstructorName { get; set; }
		public string? LearnerName { get; set; }
		public bool IsReceive { get; set; }
	}
}
