using AutoMapper;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

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

        var user = await _userRepository.GetById(request.UserId);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.GetFailed, 
                MessageCommon.Unauthorized, key, value);
        }
        var result = await _userQuestRepository.GetAllUserQuests(request.Title, request.QuestType, request.Type,
            request.PointToComplete, request.StartDate, request.DueDate,request.IsComplete, request.UserId, request.Page, request.EachPage);

        var temp = result.Items.ToList();
        List<UserQuestResponse> responseDto = new List<UserQuestResponse>();
        foreach (var item in temp)
        {
            UserQuestResponse questResponse = _mapper.Map<UserQuestResponse>(item);
            questResponse.QuestValue = ToArray(item.QuestValues!);
            questResponse.RewardType = ToArray(item.RewardTypes!);
            questResponse.RewardValue = ToArray(item.RewardValues!);
            responseDto.Add(questResponse);
        }

        PagedList<UserQuestResponse> response = new PagedList<UserQuestResponse>(responseDto, result.TotalItems, result.CurrentPage, result.EachPage);

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully,
                response, key, value);
    }
    private int[] ToArray(string values)
    {
        string[] temp = values.Split(',');
        int[] result = new int[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            result[i] = Convert.ToInt32(temp[i]);
        }
        return result;
    }
}
