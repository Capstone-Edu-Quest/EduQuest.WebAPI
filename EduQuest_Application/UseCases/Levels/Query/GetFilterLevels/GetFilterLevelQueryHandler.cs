using AutoMapper;
using EduQuest_Application.DTO.Response.Levels;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Levels.Query.GetFilterLevels;

public class GetFilterLevelQueryHandler : IRequestHandler<GetFilterLevelQuery, APIResponse>
{
    private readonly ILevelRepository _levelRepository;
    private readonly IMapper _mapper;

    public GetFilterLevelQueryHandler(ILevelRepository levelRepository, IMapper mapper)
    {
        _levelRepository = levelRepository;
        _mapper = mapper;
    }
    public async Task<APIResponse> Handle(GetFilterLevelQuery request, CancellationToken cancellationToken)
    {
        var query = await _levelRepository.GetLevelWithFiltersAsync(request.LevelNumber, request.Exp, request.Page, request.EachPage);
        List<LevelResponseDto> responseDtos = new List<LevelResponseDto>();
        

        var items = query.Items.ToList();
        foreach (var item in items)
        {
            LevelResponseDto response = new LevelResponseDto();
            response.LevelNumber = item.LevelNumber;
            response.Exp = item.Exp;
            response.RewardValue = GeneralHelper.ToArray(item.RewardValues!);
            response.RewardType = GeneralHelper.ToArray(item.RewardTypes!);
            responseDtos.Add(response); 
        }
        PagedList<LevelResponseDto> responses = new PagedList<LevelResponseDto>(responseDtos, query.Count(), query.CurrentPage, query.EachPage);
        return new APIResponse
        {
            IsError = false,
            Payload = responses,
            Message = new MessageResponse
            {
                content = MessageCommon.GetSuccesfully,
                values = new
                {
                    name = "level"
                }
            }
        };
    }
}
