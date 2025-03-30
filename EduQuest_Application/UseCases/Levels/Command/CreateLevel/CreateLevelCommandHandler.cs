using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;

namespace EduQuest_Application.UseCases.Levels.Command.CreateLevel;

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
        var isExist = await _levelRepository.CheckByLevel(request.LevelNumber);
        if (isExist)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.Conflict, Constants.MessageCommon.AlreadyExists, null, "name", "level number");
        }
        var newLevel = new Level
        {
            Id = Guid.NewGuid().ToString(),
            LevelNumber = request.LevelNumber,
            Exp = request.Exp,
            RewardTypes = GeneralHelper.ArrayToString(request.RewardType),
            RewardValues = GeneralHelper.ArrayToString(request.RewardValue),
        };

        

        await _levelRepository.Add(newLevel);
        await _unitOfWork.SaveChangesAsync();
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.CreateSuccesfully, null, "name", "level");
    }
}
