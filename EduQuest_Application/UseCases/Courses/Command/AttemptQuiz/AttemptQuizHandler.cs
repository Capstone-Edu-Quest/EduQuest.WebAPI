using AutoMapper;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.QuestEnum;

namespace EduQuest_Application.UseCases.Courses.Command.AttemptQuiz;

public class AttemptQuizHandler : IRequestHandler<AttemptQuizCommand, APIResponse>
{
    private readonly IQuizRepository _quizRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly IQuizAttemptRepository _quizAttemptRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserQuestRepository _userQuestRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserMetaRepository _userMetaRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IRedisCaching _redis;
    private readonly IStudyTimeRepository _studyTimeRepository;
    public AttemptQuizHandler(IQuizRepository quizRepository, ILessonRepository lessonRepository, IQuizAttemptRepository quizAttemptRepository,
        IMapper mapper, IUnitOfWork unitOfWork, IUserMetaRepository userMetaRepository, IUserQuestRepository userQuestRepository, 
        ICourseRepository courseRepository, IMaterialRepository materialRepository, IRedisCaching redis, IStudyTimeRepository studyTimeRepository)
    {
        _quizRepository = quizRepository;
        _lessonRepository = lessonRepository;
        _quizAttemptRepository = quizAttemptRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userQuestRepository = userQuestRepository;
        _courseRepository = courseRepository;
        _userMetaRepository = userMetaRepository;
        _materialRepository = materialRepository;
        _redis = redis;
        _studyTimeRepository = studyTimeRepository;
    }

    public async Task<APIResponse> Handle(AttemptQuizCommand request, CancellationToken cancellationToken)
    {
        int attempNo = 0;
        int CorrectQuestion = 0;
        DateTime now = DateTime.Now;
        var lesson = await _lessonRepository.GetById(request.LessonId);
        if(lesson == null)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound,
                MessageCommon.NotFound, "name", "lesson");
        }
        var materials = await _materialRepository.GetMaterialsByIds(lesson.LessonMaterials.Select(x => x.MaterialId).ToList());
        var material = materials.Where(m => m.QuizId == request.Attempt.QuizId).FirstOrDefault();
        var lessonMaterial = lesson.LessonMaterials.FirstOrDefault(m => m.MaterialId == material.Id);
        var quiz = await _quizRepository.GetQuizById(request.Attempt.QuizId);
        if(lessonMaterial == null ||quiz == null)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound,
                MessageCommon.NotFound, "name", "quiz");
        }
        var questions = quiz.Questions.ToList();
        int TotalQuestion = quiz.Questions.Count;
        QuizAttempt attempt = new QuizAttempt();
        attempt.Id = Guid.NewGuid().ToString();
        attempt.SubmitAt = now.ToUniversalTime();
        attempt.UserId = request.UserId;
        attempt.QuizId = quiz.Id;
        attempt.LessonId = lesson.Id;
        attempt.TotalTime = request.Attempt.TotalTime;
        attempt.AttemptNo = await _quizAttemptRepository.GetAttemptNo(request.Attempt.QuizId, request.LessonId) +1;
        List<UserQuizAnswers> userQuizAnswers = new List<UserQuizAnswers>();
        foreach (var item in questions)
        {
            
            var userchoose = request.Attempt.Answers.Where(t => t.QuestionId == item.Id).FirstOrDefault();
            if (userchoose == null) continue;
            var answer = item.Answers.FirstOrDefault(t => t.Id == userchoose!.AnswerId);
            
            if (answer != null && answer.IsCorrect)
            {
                CorrectQuestion += 1;
                attempt.CorrectAnswers += 1;
            }
            else if (answer != null && !answer.IsCorrect)
            {
                attempt.IncorrectAnswers += 1;
            }
            UserQuizAnswers userQuizAnswer = new UserQuizAnswers
            {
                Id = Guid.NewGuid().ToString(),
                QuestionId = item.Id,
                AnswerId = userchoose!.AnswerId,
                IsCorrect = answer.IsCorrect
            };
            userQuizAnswers.Add(userQuizAnswer);
            attempt.Answers = userQuizAnswers;
        }


        double CorrectPercentage = Math.Round((double)CorrectQuestion / TotalQuestion * 100, 2);
        attempt.Percentage = CorrectPercentage;
        await _quizAttemptRepository.Add(attempt);
        await _unitOfWork.SaveChangesAsync();
        QuizAttemptResponse response = _mapper.Map<QuizAttemptResponse>(attempt);
        response.Percentage = CorrectPercentage;
        if (Convert.ToInt32(CorrectPercentage) < quiz.PassingPercentage)
        {
            response.isPassed = false;
            return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.Complete,
            response, "name", "quiz");
        }

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
            newMaterialId = lesson.LessonMaterials.FirstOrDefault(l => l.Index == (lessonMaterial.Index +1)).MaterialId;
        }
        
        learner.TotalTime += Convert.ToInt32(material.Duration);
        
        if(learner.TotalTime > course.CourseStatistic.TotalTime)
        {
            learner.TotalTime = course.CourseStatistic.TotalTime;
        }
        learner.CurrentLessonId = newLessonId;
        learner.CurrentMaterialId = newMaterialId;
        learner.ProgressPercentage = learner.TotalTime / course.CourseStatistic.TotalTime * 100;
        if(learner.ProgressPercentage > 100)
        {
            learner.ProgressPercentage = 100;
        }
        var userMeta = await _userMetaRepository.GetByUserId(request.UserId);
        userMeta.TotalStudyTime += request.Attempt.TotalTime;
        await _userMetaRepository.Update(userMeta);

        var studyTime = await _studyTimeRepository.GetByDate(now);
        if(studyTime != null)
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
                StudyTimes = request.Attempt.TotalTime,
                Date = now.ToUniversalTime()
            });
        }
        await _redis.AddToSortedSetAsync("leaderboard:season1", request.UserId, request.Attempt.TotalTime);
        await _unitOfWork.SaveChangesAsync();


        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.QUIZ, 1);
        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.QUIZ_TIME, 1);
        response.isPassed = true;
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.Complete,
            response, "name", "quiz");
    }
}
