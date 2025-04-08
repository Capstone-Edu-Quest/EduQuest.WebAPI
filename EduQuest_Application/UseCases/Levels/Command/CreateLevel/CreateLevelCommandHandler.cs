using EduQuest_Application.DTO.Request.Level;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Level.Command.CreateLevel;

public class CreateLevelCommandHandler : IRequestHandler<CreateLevelCommand, APIResponse>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILevelRepository _levelRepository;

    public CreateLevelCommandHandler(IUnitOfWork unitOfWork, ILevelRepository levelRepository)
    {
        _unitOfWork = unitOfWork;
        _levelRepository = levelRepository;
    }

    public async Task<APIResponse> Handle(CreateLevelCommand request, CancellationToken cancellationToken)
    {        
        List<Levels> levels = new List<Levels>();
        foreach(LevelDto dto in request.Level)
        {
            if (await _levelRepository.IsLevelExist(dto.Level))
            {
                return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageError.LevelExist, "name", "level");
            }
            Levels level = new Levels
            {
                Id = Guid.NewGuid().ToString(),
                Level = dto.Level,
                Exp = dto.Exp,
                RewardTypes = GeneralHelper.ArrayToString(dto.RewardType),
                RewardValues = GeneralHelper.ArrayToString(dto.RewardValue),
                CreatedAt = DateTime.UtcNow.ToUniversalTime(),
            };
            levels.Add(level);
        }
       
        await _levelRepository.CreateRangeAsync(levels);
        await _unitOfWork.SaveChangesAsync();
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.CreateSuccesfully, null, "name", "level");
    }
}
