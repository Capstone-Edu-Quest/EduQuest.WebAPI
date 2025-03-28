using EduQuest_Application.DTO.Request.Level;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Levels.Command.UpdateLevels;

public class UpdateLevelCommand : IRequest<APIResponse>
{
    public List<LevelDto> Levels { get; set; } = new List<LevelDto>();
}
