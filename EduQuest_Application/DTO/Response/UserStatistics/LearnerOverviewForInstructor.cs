using EduQuest_Application.DTO.Response.Certificates;
using EduQuest_Application.DTO.Response.Lessons;

namespace EduQuest_Application.DTO.Response.UserStatistics
{
	public class LearnerOverviewForInstructor
	{
        public string UserId { get; set; }
        public string UserName { get; set; }
        public decimal Progress { get; set; }
        public string? CertificateId { get; set; }
        public DateTime? EnrolledDate { get; set; }
        public decimal PurchasedAmount { get; set; }
    }
}
