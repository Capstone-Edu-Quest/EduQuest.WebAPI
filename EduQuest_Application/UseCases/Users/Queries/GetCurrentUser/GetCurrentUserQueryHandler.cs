using AutoMapper;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetCurrentUserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var info = await _userRepository.GetUserByEmailAsync(request.email);
        var result = _mapper.Map<UserResponseDto>(info);
        return new APIResponse
        {
            IsError = false,
            Errors = null,
            Payload = result,
            Message = null
        };
    }
}
