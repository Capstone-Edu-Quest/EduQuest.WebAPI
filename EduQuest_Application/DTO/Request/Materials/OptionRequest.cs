using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Materials
{
	public class OptionRequest : IMapTo<Option>, IMapFrom<Option>
	{
		public string AnswerContent { get; set; }
		public bool IsCorrect { get; set; }
	}
}
