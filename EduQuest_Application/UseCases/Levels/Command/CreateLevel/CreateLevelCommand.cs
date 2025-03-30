using EduQuest_Application.DTO.Request.Level;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Levels.Command.CreateLevel;

    public class CreateLevelCommand : IRequest<APIResponse>
    {
        public int LevelNumber { get; set; }
        public int Exp { get; set; }
        public object[] RewardType { get; set; }
        public object[] RewardValue { get; set; }
    public List<LevelRewardDto> Reward { get; set; }
    }
