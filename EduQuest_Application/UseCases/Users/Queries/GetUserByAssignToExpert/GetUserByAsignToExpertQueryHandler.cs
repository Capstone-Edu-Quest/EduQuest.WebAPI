using AutoMapper;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Users.Queries.GetUserByAssignToExpert;

public class GetUserByAsignToExpertQueryHandler : IRequestHandler<GetUserByAsignToExpertQuery, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByAsignToExpertQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetUserByAsignToExpertQuery request, CancellationToken cancellationToken)
    {
        var existUsers = await _userRepository.GetUserByAssignToExpet(request.expertId);
        var mappedResult = _mapper.Map<List<UserResponseDtoForExpert>>(existUsers);

        foreach (var user in mappedResult)
        {
            var expertName = await _userRepository.GetById(request.expertId);
            if (expertName == null) {  continue; }
            user.ExpertName = expertName.Username;
        }
        return GeneralHelper.CreateSuccessResponse(
           HttpStatusCode.OK,
           MessageCommon.GetSuccesfully,
           mappedResult,
           "name",
           ""
       );
    }
}
