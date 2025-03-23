using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetUserGameInfo;

public class GetUserGameInfoQueryHandler : IRequestHandler<GetUserGameInfoQuery, APIResponse>
{
    private readonly IUserMetaRepository _userStatistic;
    private readonly IMapper _mapper;

    public GetUserGameInfoQueryHandler(IUserMetaRepository userStatistic, IMapper mapper)
    {
        _userStatistic = userStatistic;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetUserGameInfoQuery request, CancellationToken cancellationToken)
    {
        var result = await _userStatistic.GetByUserId(request.userId);
        var mappedResult = _mapper.Map<UserStatisticDto>(result);
        return new APIResponse
        {
            IsError = false,
            Errors = null,
            Payload = mappedResult,
            Message = null
        };
    }
}
