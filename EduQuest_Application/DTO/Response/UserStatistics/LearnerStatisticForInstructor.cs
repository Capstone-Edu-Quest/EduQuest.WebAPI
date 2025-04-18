using EduQuest_Application.DTO.Response.Certificates;
using EduQuest_Application.DTO.Response.Lessons;

namespace EduQuest_Application.DTO.Response.UserStatistics
{
	public class LearnerStatisticForInstructor
	{
        public string UserId { get; set; }
        public decimal Progress { get; set; }
        public string? CertificateId { get; set; }
        public List<LessonBasicResponse> Lessons { get; set; }
        public DateTime? CreatedAt { get; set; }
        public decimal Amount { get; set; }
    }
}
