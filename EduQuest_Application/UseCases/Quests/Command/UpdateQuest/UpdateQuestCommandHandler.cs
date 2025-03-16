using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.ExternalServices.QuartzService;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using System.Text;
using static EduQuest_Domain.Constants.Constants;
using static Google.Rpc.Context.AttributeContext.Types;

namespace EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement;

public class UpdateQuestCommandHandler : IRequestHandler<UpdateQuestCommand, APIResponse>
{
    private readonly IQuestRepository _achievementRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserQuestRepository _userQuestRepository;
    private readonly IUserRepository _userRepository;
    private readonly IQuartzService _quartzService;
    private const string key = "name";
    private const string value = "quest";

    public UpdateQuestCommandHandler(IQuestRepository achievementRepository, IMapper mapper, 
        IUnitOfWork unitOfWork, IUserQuestRepository userQuestRepository, 
        IUserRepository userRepository, IQuartzService quartzService)
    {
        _achievementRepository = achievementRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userQuestRepository = userQuestRepository;
        _userRepository = userRepository;
        _quartzService = quartzService;
    }

    public async Task<APIResponse> Handle(UpdateQuestCommand request, CancellationToken cancellationToken)
	{
        User? user = await _userRepository.GetById(request.UserId);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.UpdateFailed, 
                MessageCommon.Unauthorized, key, value);
        }
        Quest? updatedQuest = await _achievementRepository.GetById(request.QuestId);
        if (updatedQuest == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.UpdateFailed,
                MessageCommon.NotFound, key, value);
        }

        updatedQuest.UpdatedAt = DateTime.UtcNow.ToUniversalTime();
        updatedQuest.UpdatedBy = request.UserId;
        StringBuilder valuesBuilder = new StringBuilder();
        foreach (int value in request.Quest.QuestValue)
        {
            valuesBuilder.Append(value);
            valuesBuilder.Append(",");
        }
        // remove the last comma
        string values = valuesBuilder.ToString();
        updatedQuest.QuestValues = values.Substring(0, values.Length - 1);
        updatedQuest.QuestType = request.Quest.QuestType;
        updatedQuest.Title = request.Quest.Title;
        updatedQuest.Type = request.Quest.Type;



        List<Reward> rewards = updatedQuest.Rewards.ToList();
        foreach(var updatedReward in request.Quest.UpdatedRewards)
        {
            if(updatedReward.Id != null)
            {
                Reward temp = rewards.FirstOrDefault(r => r.Id == updatedReward.Id)!;
                temp.RewardValue = updatedReward.RewardValue;
                temp.RewardType = updatedReward.RewardType;
                rewards.Add(temp);
            } else
            {
                rewards.Add(new Reward
                {
                    RewardValue = updatedReward.RewardValue,
                    RewardType = updatedReward.RewardType
                });
            }
        }

        await _achievementRepository.Update(updatedQuest);
        if(await _unitOfWork.SaveChangesAsync() > 0)
        {
            //update all UserQuests
            await _quartzService.UpdateAllUserQuest(updatedQuest.Id);

            QuestResponse response = _mapper.Map<QuestResponse>(updatedQuest);
            response.CreatedByUser = _mapper.Map<CommonUserResponse>(user);
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, response, key, value);
        }
        return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed,
                MessageCommon.CreateFailed, key, value);
    }
}
