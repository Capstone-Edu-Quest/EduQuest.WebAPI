using AutoMapper;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
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
    private readonly IRedisCaching _redis;
    private readonly IStudyTimeRepository _studyTimeRepository;
    public AttemptAssignmentHandler(IAssignmentRepository assignmentRepository, ILessonRepository lessonRepository, IAssignmentAttemptRepository assignmentAttemptRepository, 
        IMapper mapper, IUnitOfWork unitOfWork, IUserQuestRepository userQuestRepository, ICourseRepository courseRepository, 
        IUserMetaRepository userMetaRepository, IMaterialRepository materialRepository, IRedisCaching redis, IStudyTimeRepository studyTimeRepository)
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
        _redis = redis;
        _studyTimeRepository = studyTimeRepository;
    }

    public async Task<APIResponse> Handle(AttemptAssignmentCommand request, CancellationToken cancellationToken)
    {
        int attempNo = await _assignmentAttemptRepository.GetAttemptNo(request.Attempt.AssignmentId, request.LessonId, request.UserId) + 1;
        DateTime now = DateTime.Now;
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
        attempt.CreatedAt = now.ToUniversalTime();
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
            await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.STAGE, 1);
            await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.STAGE_TIME, 1);
        }
        if (newLesson == null)
        {
            await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.COURSE, 1);
            await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.COURSE_TIME, 1);
        }
        if (lessonMaterial.Index < maxIndex)
        {
            newMaterialId = lesson.LessonMaterials.FirstOrDefault(l => l.Index == (lessonMaterial.Index + 1)).MaterialId;
        }
        
        learner.CurrentLessonId = newLessonId;
        learner.CurrentMaterialId = newMaterialId;
        
        
        if (learner.ProgressPercentage > 100)
        {
            learner.ProgressPercentage = 100;
        }
        if (attempNo <= 1)
        {
            var userMeta = await _userMetaRepository.GetByUserId(request.UserId);
            userMeta.TotalStudyTime += request.Attempt.TotalTime;
            learner.TotalTime += material.Duration;
            if (learner.TotalTime > course.CourseStatistic.TotalTime)
            {
                learner.TotalTime = course.CourseStatistic.TotalTime;
				
			}
            learner.ProgressPercentage = Math.Round((await _lessonRepository.CalculateMaterialProgressAsync(request.LessonId, material.Id, (double)course.CourseStatistic.TotalTime)) * 100, 2);
            await _userMetaRepository.Update(userMeta);
        }
		await _courseRepository.Update(course);
		var studyTime = await _studyTimeRepository.GetByDate(now);
        if (studyTime != null)
        {
            studyTime.StudyTimes += request.Attempt.TotalTime;
            await _studyTimeRepository.Update(studyTime);
        }
        else
        {
            await _studyTimeRepository.Add(new StudyTime
            {
                Id = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                StudyTimes = Convert.ToInt32(request.Attempt.TotalTime),
                Date = now.ToUniversalTime()
            });
        }
        await _redis.AddToSortedSetAsync("leaderboard:season1", request.UserId, Convert.ToInt32(request.Attempt.TotalTime));
        await _unitOfWork.SaveChangesAsync();

        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.QUIZ, 1);
        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.QUIZ_TIME, 1);
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.Complete,
            attempt, "name", "assignment");
    }
}
