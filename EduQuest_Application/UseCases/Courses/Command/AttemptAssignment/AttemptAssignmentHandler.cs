using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduQuest_Application.Helper;
using static EduQuest_Domain.Constants.Constants;
using EduQuest_Domain.Entities;
using static EduQuest_Domain.Enums.QuestEnum;

namespace EduQuest_Application.UseCases.Courses.Command.AttemptAssignment;

public class AttemptAssignmentHandler : IRequestHandler<AttemptAssignmentCommand, APIResponse>
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly IAssignmentAttemptRepository _assignmentAttemptRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserQuestRepository _userQuestRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserMetaRepository _userMetaRepository;
    private readonly IMaterialRepository _materialRepository;

    public AttemptAssignmentHandler(IAssignmentRepository assignmentRepository, ILessonRepository lessonRepository, IAssignmentAttemptRepository assignmentAttemptRepository, 
        IMapper mapper, IUnitOfWork unitOfWork, IUserQuestRepository userQuestRepository, ICourseRepository courseRepository, 
        IUserMetaRepository userMetaRepository, IMaterialRepository materialRepository)
    {
        _assignmentRepository = assignmentRepository;
        _lessonRepository = lessonRepository;
        _assignmentAttemptRepository = assignmentAttemptRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userQuestRepository = userQuestRepository;
        _courseRepository = courseRepository;
        _userMetaRepository = userMetaRepository;
        _materialRepository = materialRepository;
    }

    public async Task<APIResponse> Handle(AttemptAssignmentCommand request, CancellationToken cancellationToken)
    {
        int attempNo = await _assignmentAttemptRepository.GetAttemptNo(request.Attempt.AssignmentId, request.LessonId, request.UserId) + 1;

        var lesson = await _lessonRepository.GetById(request.LessonId);
        if (lesson == null)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound,
                MessageCommon.NotFound, "name", "lesson");
        }
        var materials = await _materialRepository.GetMaterialsByIds(lesson.LessonMaterials.Select(x => x.MaterialId).ToList());
        var material = materials.Where(m => m.AssignmentId == request.Attempt.AssignmentId).FirstOrDefault();
        var lessonMaterial = lesson.LessonMaterials.FirstOrDefault(m => m.MaterialId == material.Id);
        var assignment = await _assignmentRepository.GetById(request.Attempt.AssignmentId);
        if (lessonMaterial == null || assignment == null)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound,
                MessageCommon.NotFound, "name", "assignment");
        }
        AssignmentAttempt attempt = new AssignmentAttempt();
        attempt.Id = Guid.NewGuid().ToString();
        attempt.AttemptNo = attempNo;
        attempt.AssignmentId = request.Attempt.AssignmentId;
        attempt.UserId = request.UserId;
        attempt.AnswerContent = request.Attempt.AnswerContent;
        attempt.ToTalTime = request.Attempt.TotalTime;
        attempt.LessonId = request.LessonId;
        attempt.AnswerScore = -1;
        await _assignmentAttemptRepository.Add(attempt);

        var course = await _courseRepository.GetById(lesson.CourseId);
        var learner = course.CourseLearners.FirstOrDefault(l => l.UserId == request.UserId);
        int maxIndex = lesson.LessonMaterials.Count - 1;
        string newLessonId = lesson.Id;
        string newMaterialId = lessonMaterial.MaterialId;
        var newLesson = course.Lessons!.Where(l => l.Index == lesson.Index + 1).FirstOrDefault();
        if (lessonMaterial.Index == maxIndex && newLesson != null)
        {
            newLessonId = newLesson.Id;
            newMaterialId = newLesson.LessonMaterials.FirstOrDefault(l => l.Index == 0).MaterialId;
        }
        if (lessonMaterial.Index < maxIndex)
        {
            newMaterialId = lesson.LessonMaterials.FirstOrDefault(l => l.Index == (lessonMaterial.Index + 1)).MaterialId;
        }
        
        learner.CurrentLessonId = newLessonId;
        learner.CurrentMaterialId = newMaterialId;
        
        learner.ProgressPercentage = (decimal)learner.TotalTime / course.CourseStatistic.TotalTime * 100;
        if (learner.ProgressPercentage > 100)
        {
            learner.ProgressPercentage = 100;
        }
        if (attempNo <= 1)
        {
            var userMeta = await _userMetaRepository.GetByUserId(request.UserId);
            userMeta.TotalStudyTime += Convert.ToInt32(request.Attempt.TotalTime);
            learner.TotalTime += Convert.ToInt32(material.Duration);
            if (learner.TotalTime > course.CourseStatistic.TotalTime)
            {
                learner.TotalTime = course.CourseStatistic.TotalTime;
            }
            await _userMetaRepository.Update(userMeta);
        }
        await _unitOfWork.SaveChangesAsync();

        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.QUIZ, 1);
        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.QUIZ_TIME, 1);
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.Complete,
            attempt, "name", "assignment");
    }
}
