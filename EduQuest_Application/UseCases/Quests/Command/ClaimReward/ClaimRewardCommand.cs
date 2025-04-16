using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Quests.Command.ClaimReward;

public class ClaimRewardCommand : IRequest<APIResponse>
{
    public string UserQuestId { get; set; }
    public string UserId { get; set; }

    public ClaimRewardCommand(string userQuestId, string userId)
    {
        UserQuestId = userQuestId;
        UserId = userId;
    }
}
