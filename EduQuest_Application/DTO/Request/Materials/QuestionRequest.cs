using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Materials
{
	public class QuestionRequest : IMapFrom<Question>, IMapTo<Question>
	{
		public string QuestionTitle { get; set; }
		public bool MultipleAnswers { get; set; }
        public List<AnswerRequest> AnswerRequest { get; set; }
    }
}
