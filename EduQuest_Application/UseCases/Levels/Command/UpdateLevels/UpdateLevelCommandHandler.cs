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
        var updatedLevels = new List<Level>();
        var exist = await _levelRepository.GetAll();
        foreach (var eachLevel in request.Levels)
        {
            var existingLevel = exist.FirstOrDefault(x => x.LevelNumber == eachLevel.Level);
            if (existingLevel == null)
            {
                continue;
            }
            existingLevel.Exp = eachLevel.Exp;
            if (existingLevel.LevelRewards != null && existingLevel.LevelRewards.Any())
            {
               _levelRewardRepository.DeleteRange(existingLevel.LevelRewards.ToList());
            }

            var newRewards = eachLevel.Rewards.Select(r => new LevelReward
            {
                LevelId = existingLevel.ToString()!,
                RewardType = r.RewardType,
                RewardValue = r.RewardValue,
                Level = existingLevel
            }).ToList();

            existingLevel.LevelRewards = newRewards;

            updatedLevels.Add(existingLevel);
        }
        await _levelRepository.UpdateRangeAsync(updatedLevels);
        await _unitOfWork.SaveChangesAsync();
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.UpdateSuccesfully, null, "name", "level");
    }
}
