using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using Google.Cloud.Firestore.V1;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

        var result = await _questRepository.GetAllQuests(request.Title, request.Description, request.PointToComplete,
            request.Type, request.TimeToComplete, request.Page, request.EachPage);

        var temp = result.Items.ToList();
        List<QuestResponse> responseDto = new List<QuestResponse>();
        foreach (var item in temp)
        {
            CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(item.User);
            QuestResponse questResponse = _mapper.Map<QuestResponse>(item);
            questResponse.CreatedByUser = userResponse;
            responseDto.Add(questResponse);
        }
        PagedList<QuestResponse> response = new PagedList<QuestResponse>(responseDto, result.TotalItems, result.CurrentPage, result.EachPage);

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully,
                response, key, value);
    }
}
