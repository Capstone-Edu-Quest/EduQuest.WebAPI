using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Query.GetAssignmentAttempt;

public class GetAssignmentAttemptHandler : IRequestHandler<GetAssignmentAttemptCommand, APIResponse>
{
    private readonly IAssignmentAttemptRepository _assignmentAttemptRepository;
    private readonly ILessonMaterialRepository _lessonMaterialRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;

    public GetAssignmentAttemptHandler(IAssignmentAttemptRepository assignmentAttemptRepository, ILessonMaterialRepository lessonMaterialRepository,
        IMaterialRepository materialRepository, IMapper mapper)
    {
        _assignmentAttemptRepository = assignmentAttemptRepository;
        _lessonMaterialRepository = lessonMaterialRepository;
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetAssignmentAttemptCommand request, CancellationToken cancellationToken)
    {
        var lesson = await _lessonMaterialRepository.GetListMaterialIdByLessonId(request.LessonId);
        var attempt = await _assignmentAttemptRepository.GetLearnerAttempt(request.AssignmentId, request.UserId);
        if (attempt == null)
        {
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.NotAttemptRecorded, null, "name", "assignment attempt");
        }
        AssignmentAttemptResponse response = _mapper.Map<AssignmentAttemptResponse>(attempt);
        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", "assignment attempt");
    }
}
