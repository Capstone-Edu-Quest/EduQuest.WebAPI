using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Materials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
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
    private readonly ICourseRepository _courseRepository;

    public GetAssignmentAttemptsForInsHandler(ILessonRepository lessonRepository, IMaterialRepository materialRepository, 
        IAssignmentAttemptRepository assignmentAttemptRepository, IMapper mapper, ICourseRepository courseRepository)
    {
        _lessonRepository = lessonRepository;
        _materialRepository = materialRepository;
        _assignmentAttemptRepository = assignmentAttemptRepository;
        _mapper = mapper;
        _courseRepository = courseRepository;
    }

    public async Task<APIResponse> Handle(GetAssignmentAttemptsForInsQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetById(request.courseId);
        if(course == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", $"course with id{request.courseId}");
        }

        var lessons = course.Lessons;

        List<LessonMaterial> lessonMaterials = new List<LessonMaterial>();
        foreach( var lesson in lessons)
        {
            lessonMaterials.AddRange(lesson.LessonMaterials);
        }
        List<string> materialIds = new List<string>();
        foreach(var lesson in lessons)
        {
            List<string> temp = lesson.LessonMaterials.Select(l => l.MaterialId).ToList();
            materialIds.AddRange(temp);
        }
        var materials = await _materialRepository.GetMaterialsByIds(materialIds);
        materials = materials.Where(m => m.AssignmentId != null).ToList();

        List<AssignmentResponse> responses = new List< AssignmentResponse >();
        foreach (var material in materials)
        {
            var lessonId = lessonMaterials.Where(l => l.MaterialId == material.Id).FirstOrDefault().LessonId;
            var assignment = material.Assignment;
            var assignmentAttempts = await _assignmentAttemptRepository.GetUnreviewedAttempts("lesson", assignment!.Id);
            List<AssignmentAttemptResponseForInstructor> attempts = _mapper.Map<List<AssignmentAttemptResponseForInstructor>>(assignmentAttempts);
            AssignmentResponse dto = _mapper.Map<AssignmentResponse>(assignment);
            dto.attempts = attempts;
            responses.Add(dto);
        }
        
        


        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully, responses, "name", "assignment");
    }
}
