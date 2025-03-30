using EduQuest_Application.DTO.Request.Level;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Levels.Command.UpdateLevels;

public class UpdateLevelCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public List<LevelDto> Levels { get; set; } = new List<LevelDto>();

    public UpdateLevelCommand(string userId, List<LevelDto> levels)
    {
        UserId = userId;
        Levels = levels;
    }
}
