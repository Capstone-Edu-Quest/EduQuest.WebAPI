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
using System.Text.Json;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement;

public class UpdateQuestCommandHandler : IRequestHandler<UpdateQuestCommand, APIResponse>
{
    private readonly IQuestRepository _achievementRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IQuartzService _quartzService;
    private const string key = "name";
    private const string value = "quest";

    public UpdateQuestCommandHandler(IQuestRepository achievementRepository, IMapper mapper, 
        IUnitOfWork unitOfWork, IUserRepository userRepository, IQuartzService quartzService)
    {
        _achievementRepository = achievementRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
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
        Quest? updatedQuest = await _achievementRepository.GetById(request.Quest.Id);
        if (updatedQuest == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.UpdateFailed,
                MessageCommon.NotFound, key, value);
        }

        updatedQuest.UpdatedAt = DateTime.Now.ToUniversalTime();
        updatedQuest.UpdatedBy = request.UserId;
        updatedQuest.QuestType = request.Quest.QuestType;
        updatedQuest.Title = request.Quest.Title;
        updatedQuest.Type = request.Quest.Type;

        updatedQuest.QuestValues = GeneralHelper.ArrayToString(request.Quest.QuestValue);
        updatedQuest.RewardValues = GeneralHelper.ArrayToString(request.Quest.RewardValue);
        updatedQuest.RewardTypes = GeneralHelper.ArrayToString(request.Quest.RewardType);

        await _achievementRepository.Update(updatedQuest);
        if(await _unitOfWork.SaveChangesAsync() > 0)
        {
            //update all UserQuests
            await _quartzService.UpdateAllUserQuest(updatedQuest.Id);

            QuestResponse response = _mapper.Map<QuestResponse>(updatedQuest);
            response.CreatedByUser = _mapper.Map<CommonUserResponse>(updatedQuest.User);
            response.QuestValue = request.Quest.QuestValue;
            response.RewardType = request.Quest.RewardType;
            response.RewardValue = request.Quest.RewardValue;
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, response, key, value);
        }
        return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed,
                MessageCommon.CreateFailed, key, value);
    }

}
