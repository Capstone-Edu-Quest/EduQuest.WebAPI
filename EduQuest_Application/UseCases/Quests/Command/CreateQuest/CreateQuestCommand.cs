using EduQuest_Application.DTO.Request.Quests;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Quests.Command.CreateQuest
{
    public class CreateQuestCommand : IRequest<APIResponse>
	{
		public string UserId { get; set; }
        public CreateQuestRequest Quest { get; set; }

		public CreateQuestCommand(string userId, CreateQuestRequest quest)
		{
			UserId = userId;
			Quest = quest;
		}
	}
}
