using EduQuest_Application.DTO.Request.Level;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Level.Command.CreateLevel;

    public class CreateLevelCommand : IRequest<APIResponse>
    {
        public List<LevelDto> Level {  get; set; }

    public CreateLevelCommand(List<LevelDto> levelDto)
    {
        Level = levelDto;
    }
}
