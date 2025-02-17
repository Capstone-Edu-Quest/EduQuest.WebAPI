using AutoMapper;
using EduQuest_Application.DTO.Response.Badges;
using EduQuest_Application.UseCases.Badges.Queries;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

public class GetBadgesQueryHandler : IRequestHandler<GetBadgesQuery, APIResponse>
{
    private readonly IBadgeRepository _badgeRepository;
    private readonly IMapper _mapper;

    public GetBadgesQueryHandler(IBadgeRepository badgeRepository, IMapper mapper)
    {
        _badgeRepository = badgeRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetBadgesQuery request, CancellationToken cancellationToken)
    {
        var query = _badgeRepository.GetBadgesWithFilters(request.Name, request.Description, request.IconUrl, request.Color);

        var badges = _mapper.Map<IEnumerable<BadgeDto>>(query.ToList());

        return new APIResponse
        {
            IsError = false,
            Payload = badges,
            Message = new MessageResponse
            {
                content = MessageCommon.GetSuccesfully,
                values = "badges"
            }
        };
    }
}
