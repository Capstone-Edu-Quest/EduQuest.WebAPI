using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Quests.Queries.GetAllSystemQuests;

public class GetAllSystemQuestsHandler : IRequestHandler<GetAllSystemQuestsQuery, APIResponse>
{
    private readonly IQuestRepository _questRepository;
    private readonly IMapper _mapper;
    private const string key = "name";
    private const string value = "quest";

    public GetAllSystemQuestsHandler(IQuestRepository questRepository, IMapper mapper)
    {
        _questRepository = questRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetAllSystemQuestsQuery request, CancellationToken cancellationToken)
    {

        var result = await _questRepository.GetAllQuests(request.Title, request.QuestType, request.Type, request.QuestValue,
            request.UserId, request.Page, request.EachPage);

        var temp = result.Items.ToList();
        List<QuestResponse> responseDto = new List<QuestResponse>();
        foreach (var item in temp)
        {
            CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(item.User);
            QuestResponse questResponse = _mapper.Map<QuestResponse>(item);
            questResponse.QuestValue = GeneralHelper.ToArray(item.QuestValues!);
            questResponse.RewardType = GeneralHelper.ToArray(item.RewardTypes!);
            questResponse.RewardValue = GeneralHelper.ToArray(item.RewardValues!);
            questResponse.CreatedByUser = userResponse;
            responseDto.Add(questResponse);
        }
        PagedList<QuestResponse> response = new PagedList<QuestResponse>(responseDto, result.TotalItems, result.CurrentPage, result.EachPage);

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully,
                response, key, value);
    }
    /*private int[] ToArray(string values)
    {
        string[] temp = values.Split(',');
        int[] result = new int[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            result[i] = Convert.ToInt32(temp[i]);
        }
        return result;
    }*/
    private object[] ToArray(string values)
    {
        string[] temp = values.Split(',');
        object[] result = new object[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            try
            {
                result[i] = Convert.ToInt32(temp[i]);
            }catch (Exception)
            {
                result[i] = temp[i];
            }
        }
        return result;
    }
}
