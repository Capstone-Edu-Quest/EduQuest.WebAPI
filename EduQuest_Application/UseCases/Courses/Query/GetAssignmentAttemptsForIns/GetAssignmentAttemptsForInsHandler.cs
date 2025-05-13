using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Materials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Query.GetAssignmentAttemptsForIns;

public class GetAssignmentAttemptsForInsHandler : IRequestHandler<GetAssignmentAttemptsForInsQuery, APIResponse>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IAssignmentAttemptRepository _assignmentAttemptRepository;
    private readonly IMapper _mapper;
    private readonly ICourseRepository _courseRepository;
    private readonly IAssignmentRepository _assignmentRepository;

    public GetAssignmentAttemptsForInsHandler(IMaterialRepository materialRepository, IAssignmentAttemptRepository assignmentAttemptRepository, 
        IMapper mapper, ICourseRepository courseRepository, IAssignmentRepository assignmentRepository)
    {
        _materialRepository = materialRepository;
        _assignmentAttemptRepository = assignmentAttemptRepository;
        _mapper = mapper;
        _courseRepository = courseRepository;
        _assignmentRepository = assignmentRepository;
    }

    public async Task<APIResponse> Handle(GetAssignmentAttemptsForInsQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetById(request.courseId);
        if(course == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", $"course with id{request.courseId}");
        }

        var lessons = course.Lessons;

        List<LessonContent> lessonMaterials = new List<LessonContent>();
        foreach( var lesson in lessons)
        {
            lessonMaterials.AddRange(lesson.LessonMaterials);
        }
        List<string> AssignmentIds = new List<string>();
        foreach(var lesson in lessons)
        {
            List<string> temp = lesson.LessonMaterials.Select(l => l.AssignmentId!).ToList();
            AssignmentIds.AddRange(temp);
        }
        //var materials = await _materialRepository.GetMaterialsByIds(materialIds);

        //materials = materials.Where(m => m.AssignmentId != null).ToList();

        UnreviewedAssignmentAttempt responseDto = new UnreviewedAssignmentAttempt();
        List<AssignmentResponse> responses = new List< AssignmentResponse >();
        foreach (var assignmentId in AssignmentIds)
        {
            var lessonId = lessonMaterials.Where(l => l.AssignmentId == assignmentId).FirstOrDefault().LessonId;
            var assignment = await _assignmentRepository.GetById(assignmentId);
            var assignmentAttempts = await _assignmentAttemptRepository.GetUnreviewedAttempts(lessonId, assignmentId);
            List<AssignmentAttemptResponseForInstructor> attempts = _mapper.Map<List<AssignmentAttemptResponseForInstructor>>(assignmentAttempts);
            AssignmentResponse dto = _mapper.Map<AssignmentResponse>(assignment);
            dto.attempts = attempts;
            dto.LessonName = lessons.FirstOrDefault(l => l.Id == lessonId).Name;
            dto.LessonIndex = lessons.FirstOrDefault(l => l.Id == lessonId).Index;
            if (dto.attempts.Count > 0)
            {
                responses.Add(dto);
            }
        }
        responseDto.CourseName = course.Title;
        responseDto.CourseId = course.Id;
        responseDto.Assignments = responses;
        


        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully, responseDto, "name", "assignment");
    }
}
