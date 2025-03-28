using EduQuest_Application.DTO.Request.Level;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;

namespace EduQuest_Application.UseCases.Levels.Command.UpdateLevels;

public class UpdateLevelCommandHandler : IRequestHandler<UpdateLevelCommand, APIResponse>
{
    private readonly ILevelRepository _levelRepository;
    private readonly ILevelRewardRepository _levelRewardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLevelCommandHandler(ILevelRepository levelRepository, ILevelRewardRepository levelRewardRepository, IUnitOfWork unitOfWork)
    {
        _levelRepository = levelRepository;
        _levelRewardRepository = levelRewardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(UpdateLevelCommand request, CancellationToken cancellationToken)
    {
        var listOfLevel = request.Levels.Select(a => a.LevelNumber).ToList();
        var existOfLevel = await _levelRepository.GetByBatchLevelNumber(listOfLevel);
        foreach (var eachLevel in existOfLevel)
        {
            var requestLevel = request.Levels.FirstOrDefault(x => x.LevelNumber == eachLevel.LevelNumber);
            if (requestLevel == null) continue;

            eachLevel.Exp = requestLevel.Exp;
            var oldRewards = await _levelRewardRepository.GetByLevelIdAsync(eachLevel.Id);
            if (oldRewards.Any())
            {
                _levelRewardRepository.RemoveRangeAsync(oldRewards);
                await _unitOfWork.SaveChangesAsync(); 
            }

            var newRewards = requestLevel.Rewards.Select(r => new LevelReward
            {
                Id = Guid.NewGuid().ToString(),
                LevelId = eachLevel.Id,
                RewardType = r.RewardType,
                RewardValue = r.RewardValue,
                Level = eachLevel
            }).ToList();

            await _levelRewardRepository.CreateRangeAsync(newRewards);
        }
        await _unitOfWork.SaveChangesAsync();
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.UpdateSuccesfully, null, "name", "level");
    }
}
