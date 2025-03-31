using EduQuest_Application.DTO.Request.Level;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Level.Command.CreateLevel;

    public class CreateLevelCommand : IRequest<APIResponse>
    {
        public LevelDto Level {  get; set; }

    public CreateLevelCommand( LevelDto levelDto)
    {
        Level = levelDto;
    }
}
