using EduQuest_Application.DTO.Request.Level;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Level.Command.UpdateLevels;

public class UpdateLevelCommandHandler : IRequestHandler<UpdateLevelCommand, APIResponse>
{
    private readonly ILevelRepository _levelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLevelCommandHandler(ILevelRepository levelRepository, IUnitOfWork unitOfWork)
    {
        _levelRepository = levelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(UpdateLevelCommand request, CancellationToken cancellationToken)
    {
        List<string> listOfLevel = request.Levels.Select(a => a.Id).ToList();

        var existOfLevel = await _levelRepository.GetByBatchLevelNumber(listOfLevel!);
        foreach (var eachLevel in existOfLevel)
        {
            var requestLevel = request.Levels.FirstOrDefault(x => x.Id.ToString() == eachLevel.Id);
            if (requestLevel == null) continue;

            eachLevel.Exp = requestLevel.Exp;
            eachLevel.RewardTypes = GeneralHelper.ArrayToString(requestLevel.RewardType);
            eachLevel.RewardValues = GeneralHelper.ArrayToString(requestLevel.RewardValue);
            eachLevel.UpdatedAt = DateTime.Now.ToUniversalTime();
            eachLevel.UpdatedBy = request.UserId;
        }


        var newLevel = request.Levels.Where(l => string.IsNullOrEmpty(l.Id)).ToList();
        List<Levels> levels = new List<Levels>();
        foreach (LevelDto dto in newLevel)
        {
            if (await _levelRepository.IsLevelExist(dto.Level))
            {
                return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageError.LevelExist, "name", $"level {dto.Level}");
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
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.UpdateSuccesfully, null, "name", "level");
    }
}
