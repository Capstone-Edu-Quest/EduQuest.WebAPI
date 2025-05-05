using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Revenue
{
	public class DetailRevenueTransaction : IMapFrom<TransactionDetail>, IMapTo<TransactionDetail>
	{
		public string Id { get; set; }
		//public string TransactionId { get; set; }
		//public string Title { get; set; }
		public DateTime? UpdatedAt { get; set; }
        public string UserName { get; set; }
        public DateTime? ReceiveDate { get; set; }
		//public string ItemId { get; set; }
		public decimal Amount { get; set; }
		public decimal? StripeFee { get; set; }
		public decimal? NetAmount { get; set; }
		public decimal? SystemShare { get; set; }
		public decimal? InstructorShare { get; set; }
        public string TransferId { get; set; }
       
    }
}
