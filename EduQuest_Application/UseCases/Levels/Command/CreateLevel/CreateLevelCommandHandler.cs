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
        int newLevelId = await _levelRepository.CountAsync() + 1;
        var newLevel = new Level
        {
            Id = newLevelId.ToString(),
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
