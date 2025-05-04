using AutoMapper;
using EduQuest_Application.DTO.Response.Profiles;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Users.Queries.GetMyInstructorApplicationQuery;

public class GetMyInstructorApplicationQueryHandler : IRequestHandler<GetMyInstructorApplicationQuery, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetMyInstructorApplicationQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetMyInstructorApplicationQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.userId);

        //if (user == null || user.Status != "Pending" || user.Status != "Rejected")
        //{
        //    return GeneralHelper.CreateErrorResponse(
        //        HttpStatusCode.NotFound,
        //        MessageCommon.HaventApplyInstructor,
        //        MessageCommon.HaventApplyInstructor,
        //        "name",
        //        request.userId
        //    );
        //}

        var result = _mapper.Map<InstructorProfileDto>(user);
        if (!string.IsNullOrEmpty(user.AssignToExpertId))
        {
            var expertUser = await _userRepository.GetById(user.AssignToExpertId);
            if (expertUser != null)
            {
                result.ExpertName = expertUser.Username;
            }
        }

        return GeneralHelper.CreateSuccessResponse(
            HttpStatusCode.OK,
            MessageCommon.GetSuccesfully,
            result,
            "user",
            "status"
        );
    }
}
