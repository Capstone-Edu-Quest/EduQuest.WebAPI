﻿using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.ExternalServices.QuartzService;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Quests.Command.CreateQuest;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using System.Text;
using System.Text.Json;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.QuestEnum;

namespace EduQuest_Application.UseCases.Achievements.Commands.CreateAchievement;

public class CreateQuestCommandHandler : IRequestHandler<CreateQuestCommand, APIResponse>
{
    private readonly IQuestRepository _achievementRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IQuartzService _quartzService;
    private const string key = "name";
    private const string value = "quest";

    public CreateQuestCommandHandler(IQuestRepository achievementRepository, IMapper mapper, IUnitOfWork unitOfWork,
        IUserRepository userRepository,IQuartzService qquartzService)
    {
        _achievementRepository = achievementRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _quartzService = qquartzService;
    }

    public async Task<APIResponse> Handle(CreateQuestCommand request, CancellationToken cancellationToken)
    {
        #region Validation
        User? user = await _userRepository.GetById(request.UserId);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.CreateFailed,
                MessageCommon.Unauthorized, key, value);
        }

        if(request.Quest.QuestType < (int)QuestType.STAGE ||
           request.Quest.QuestType > (int)QuestType.STREAK ||
           request.Quest.Type > (int)ResetType.OneTime ||
           request.Quest.Type < (int)ResetType.Daily)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed,
                MessageQuest.InvalidQuestTypeOrResetType, key, value);
        }
        #endregion

        var questEntity = _mapper.Map<Quest>(request.Quest);

        if (request.Quest.QuestValue != null && request.Quest.QuestValue.Any())
        {
            questEntity.QuestValues = GeneralHelper.ArrayToString(request.Quest.QuestValue);
        }

        if (request.Quest.RewardValue != null && request.Quest.RewardValue.Any())
        {
            questEntity.RewardValues = GeneralHelper.ArrayToString(request.Quest.RewardValue);
        }

        if (request.Quest.RewardType != null && request.Quest.RewardType.Any())
        {
            questEntity.RewardTypes = GeneralHelper.ArrayToString(request.Quest.RewardType);
        }


        questEntity.Id = Guid.NewGuid().ToString();
        questEntity.CreatedAt = DateTime.Now.ToUniversalTime();
        questEntity.CreatedBy = request.UserId;

        await _achievementRepository.Add(questEntity);
        var result = await _unitOfWork.SaveChangesAsync() > 0;
        if (result)
        {
            //add quest to all user.
            await _quartzService.AddNewQuestToAllUser(questEntity.Id);
            //await _userQuestRepository.AddNewQuestToAllUseQuest(questEntity);

            //Create response
            QuestResponse response = _mapper.Map<QuestResponse>(questEntity);
            response.QuestValue = request.Quest.QuestValue!;
            response.RewardValue = request.Quest.RewardValue!;
            response.RewardType = request.Quest.RewardType!;
            response.CreatedByUser = _mapper.Map<CommonUserResponse>(user);
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.CreateSuccesfully, response, key, value);
        }
        return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed,
        MessageCommon.CreateFailed, key, value);
    }

    
}
