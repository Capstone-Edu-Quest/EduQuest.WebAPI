using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetEnrolledLearners;

public class GetEnrolledLearnersHandler : IRequestHandler<GetEnrolledLearnersQuery, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetEnrolledLearnersHandler(ILearningPathRepository learningPathRepository, IUserRepository userRepository, IMapper mapper)
    {
        _learningPathRepository = learningPathRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetEnrolledLearnersQuery request, CancellationToken cancellationToken)
    {
        var learningPath = await _learningPathRepository.GetById(request.LearningPathId);
        if(learningPath == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound,
                "name", "learning path");
        }
        var ids = learningPath.Enrollers.DistinctBy(e => e.UserId).Select(e => e.UserId).ToList();
        var users = await _userRepository.GetByUserIds(ids);
        List<CommonUserResponse> responses = _mapper.Map<List<CommonUserResponse>>(users);

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully,
            responses, "name", "enrollers");
    }
}
