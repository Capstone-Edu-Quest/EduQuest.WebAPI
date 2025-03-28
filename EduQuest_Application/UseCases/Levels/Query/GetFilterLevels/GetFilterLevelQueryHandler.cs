using AutoMapper;
using EduQuest_Application.DTO.Response.Levels;
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

        var certificates = _mapper.Map<PagedList<LevelResponseDto>>(query);

        return new APIResponse
        {
            IsError = false,
            Payload = certificates,
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
