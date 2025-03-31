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
        if(await _levelRepository.IsLevelExist(request.Level.Level))
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageError.LevelExist, "name", "level");
        }
        var newLevel = new EduQuest_Domain.Entities.Levels
        {
            Id = Guid.NewGuid().ToString(),
            Level = request.Level.Level,
            Exp = request.Level.Exp,
            RewardTypes = GeneralHelper.ArrayToString(request.Level.RewardType),
            RewardValues = GeneralHelper.ArrayToString(request.Level.RewardValue),
            CreatedAt = DateTime.UtcNow.ToUniversalTime(),
            
        };
        await _levelRepository.Add(newLevel);
        await _unitOfWork.SaveChangesAsync();
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.CreateSuccesfully, null, "name", "level");
    }
}
