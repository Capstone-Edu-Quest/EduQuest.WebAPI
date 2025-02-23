using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Materials
{
	public class AssignmentRequest : IMapFrom<Assignment>, IMapTo<Assignment>
	{
		public int? TimeLimit { get; set; }
		public string? Question { get; set; }
		public string? AnswerLanguage { get; set; }
		public string? ExpectedAnswer { get; set; }

		
	}
}
