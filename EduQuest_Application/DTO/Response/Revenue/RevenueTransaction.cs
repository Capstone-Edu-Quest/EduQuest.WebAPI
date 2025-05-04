using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Revenue
{
	public class RevenueTransaction : IMapFrom<TransactionDetail>, IMapTo<TransactionDetail>
	{
        public string TransactionId { get; set; }
		public string Title { get; set; }
		public DateTime? Date { get; set; }
		public decimal Amount { get; set; }
		public decimal? StripeFee { get; set; }
		public decimal? NetAmount { get; set; }
		public decimal? SystemShare { get; set; }
		public decimal? InstructorShare { get; set; }
        public bool IsReceive { get; set; }
		public DateTime? ReceiveDate { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<TransactionDetail, RevenueTransaction>()
				.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.UpdatedAt));
		}
	}
}
