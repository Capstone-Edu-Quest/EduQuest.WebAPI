using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Materials;
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

namespace EduQuest_Application.UseCases.Courses.Query.GetAssignmentAttemptsForIns;

public class GetAssignmentAttemptsForInsHandler : IRequestHandler<GetAssignmentAttemptsForInsQuery, APIResponse>
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IAssignmentAttemptRepository _assignmentAttemptRepository;
    private readonly IMapper _mapper;

    public GetAssignmentAttemptsForInsHandler(ILessonRepository lessonRepository, IMaterialRepository materialRepository, 
        IAssignmentAttemptRepository assignmentAttemptRepository, IMapper mapper)
    {
        _lessonRepository = lessonRepository;
        _materialRepository = materialRepository;
        _assignmentAttemptRepository = assignmentAttemptRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetAssignmentAttemptsForInsQuery request, CancellationToken cancellationToken)
    {
        var lesson = await _lessonRepository.GetById(request.LessonId);
        var material = await _materialRepository.GetMaterialByAssignmentId(request.AssignmentId);
        if(material == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound,MessageCommon.NotFound, "name", "assignment");
        }
        string? lessonMaterialId = lesson.LessonMaterials.Where(lm => lm.MaterialId == material.Id).FirstOrDefault().MaterialId;
        if(lessonMaterialId == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", "assignment");
        }

        var assignment = material.Assignment;
        var assignmentAttempts = await _assignmentAttemptRepository.GetUnreviewedAttempts(request.LessonId, request.AssignmentId);
        List<AssignmentAttemptResponseForInstructor> attempts = _mapper.Map<List<AssignmentAttemptResponseForInstructor>>(assignmentAttempts);
        AssignmentResponse response = _mapper.Map<AssignmentResponse>(assignment);
        response.attempts = attempts;
        


        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", "assignment");
    }
}
