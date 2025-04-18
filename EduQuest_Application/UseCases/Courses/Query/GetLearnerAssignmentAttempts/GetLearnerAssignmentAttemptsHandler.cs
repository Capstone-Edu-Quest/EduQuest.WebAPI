using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Query.GetLearnerAssignmentAttempts;

public class GetLearnerAssignmentAttemptsHandler : IRequestHandler<GetLearnerAssignmentAttemptsQuery, APIResponse>
{
    private readonly IAssignmentAttemptRepository _assignmentAttemptRepository;
    private readonly IMapper _mapper;

    public GetLearnerAssignmentAttemptsHandler(IAssignmentAttemptRepository assignmentAttemptRepository, IMapper mapper)
    {
        _assignmentAttemptRepository = assignmentAttemptRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetLearnerAssignmentAttemptsQuery request, CancellationToken cancellationToken)
    {
        var attempts = await _assignmentAttemptRepository.GetLearnerAttempts(request.LessonId, request.AssignmentId);
        List<AssignmentAttemptResponseForInstructor> response = _mapper.Map<List<AssignmentAttemptResponseForInstructor>>(attempts);

        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully,
            response, "name", "assignment attempts");
    }
}
