using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;
using static Google.Rpc.Context.AttributeContext.Types;

namespace EduQuest_Application.UseCases.Quests.Queries.GetAllUserQuests;

internal class GetAllUserQuestsHandler : IRequestHandler<GetAllUserQuestsQuery, APIResponse>
{
    private readonly IUserQuestRepository _userQuestRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private const string key = "name";
    private const string value = "user quest";

    public GetAllUserQuestsHandler(IUserQuestRepository userQuestRepository,
        IUserRepository userRepository, IMapper mapper)
    {
        _userQuestRepository = userQuestRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetAllUserQuestsQuery request, CancellationToken cancellationToken)
    {

        var user = await _userQuestRepository.GetById(request.UserId);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.GetFailed, 
                MessageCommon.Unauthorized, key, value);
        }
        var result = await _userQuestRepository.GetAllUserQuests(request.Title, request.Description, request.PointToComplete,
            request.Type, request.StartDate, request.DueDate, request.Page, request.EachPage, request.UserId);

        var temp = result.Items.ToList();
        List<UserQuestResponse> responseDto = new List<UserQuestResponse>();
        foreach (var item in temp)
        {
            UserQuestResponse questResponse = _mapper.Map<UserQuestResponse>(item);
            List<string> rewardIds = item.Rewards.Select(r => r.QuestRewardId).ToList()!;
            var rewards = await _userQuestRepository.GetUserQuestRewardAsync(rewardIds);
            List<QuestRewardResponse> questRewardResponse = _mapper.Map<List<QuestRewardResponse>>(rewards);
            questResponse.QuestRewards = questRewardResponse;
            responseDto.Add(questResponse);
        }

        PagedList<UserQuestResponse> response = new PagedList<UserQuestResponse>(responseDto, result.TotalItems, result.CurrentPage, result.EachPage);

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully,
                response, key, value);
    }
}
