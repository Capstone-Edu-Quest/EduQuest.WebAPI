using EduQuest_Application.DTO.Request.Quests;
using EduQuest_Domain.Models.Response;
using MediatR;


namespace EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement
{
    public class UpdateQuestCommand : IRequest<APIResponse>
	{
		public string UserId { get; set; }
		public UpdateQuestRequest Quest { get; set; }

		public UpdateQuestCommand(string userId, UpdateQuestRequest quest)
		{
			UserId = userId;
			Quest = quest;
		}
	}
}
