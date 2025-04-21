using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using static EduQuest_Domain.Enums.GeneralEnums;
using EduQuest_Application.Helper;
using static EduQuest_Domain.Enums.QuestEnum;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.UseCases.UserMetas.Commands.UpdateUsersStreak;

namespace EduQuest_Application.UseCases.UserMetas.Commands.UpdateUserProgress
{
	public class UpdateUserProgressCommandHandler : IRequestHandler<UpdateUserProgressCommand, APIResponse>
	{
		private readonly IUserMetaRepository _userMetaRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICourseRepository _courseRepository;
		private readonly ILessonRepository _lessonRepository;
		private readonly IMaterialRepository _materialRepository;
		private readonly ISystemConfigRepository _systemConfigRepository;
		private readonly IUserQuestRepository _userQuestRepository;
		private readonly IRedisCaching _redis;
		private readonly IStudyTimeRepository _studyTimeRepository;
		private readonly IMediator mediator;

        public UpdateUserProgressCommandHandler(IUserMetaRepository userMetaRepository, IUnitOfWork unitOfWork, ICourseRepository courseRepository, ILessonRepository lessonRepository, IMaterialRepository materialRepository, ISystemConfigRepository systemConfigRepository, IUserQuestRepository userQuestRepository, IRedisCaching redis, IStudyTimeRepository studyTimeRepository, IMediator mediator)
        {
            _userMetaRepository = userMetaRepository;
            _unitOfWork = unitOfWork;
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
            _materialRepository = materialRepository;
            _systemConfigRepository = systemConfigRepository;
            _userQuestRepository = userQuestRepository;
            _redis = redis;
            _studyTimeRepository = studyTimeRepository;
            this.mediator = mediator;
        }

        public async Task<APIResponse> Handle(UpdateUserProgressCommand request, CancellationToken cancellationToken)
		{
			DateTime now = DateTime.Now;
			var userMeta = await _userMetaRepository.GetByUserId(request.UserId);
			//Get material
			var material = await _materialRepository.GetMataterialQuizAssById(request.Info.MaterialId);
			//Get lesson
			var lesson = await _lessonRepository.GetById(request.Info.LessonId);

			//Get CourseLeaner
			var course = await _courseRepository.GetById(lesson.CourseId);
			var courseLearner = course.CourseLearners.FirstOrDefault(x => x.UserId == request.UserId);
			if(courseLearner == null)
			{
				return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageLearner.NotLearner, $"Not Found", "name", $"Not Found in Course ID {lesson.CourseId}");
			}

            var studyTime = await _studyTimeRepository.GetByDate(now);
            if (studyTime != null)
            {
                studyTime.StudyTimes += (double)request.Info.Time != null ? (double)request.Info.Time : (double)material.Duration;
                await _studyTimeRepository.Update(studyTime);
            }
            else
            {
                await _studyTimeRepository.Add(new StudyTime
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = request.UserId,
                    StudyTimes = ((double)(request.Info.Time != null ? request.Info.Time.Value : material.Duration)),
                    Date = now.ToUniversalTime()
                });
            }
            if (request.Info.Time != null)
			{
                courseLearner.TotalTime += material.Duration;
				userMeta.TotalStudyTime += request.Info.Time;
                await _redis.AddToSortedSetAsync("leaderboard:season1", request.UserId, userMeta.TotalStudyTime.Value);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME, request.Info.Time.Value);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME_TIME, request.Info.Time.Value);
            } else
			{
				courseLearner.TotalTime += material.Duration;
				userMeta.TotalStudyTime += material.Duration;
                await _redis.AddToSortedSetAsync("leaderboard:season1", request.UserId, userMeta.TotalStudyTime.Value);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME, (int)material.Duration);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME_TIME, (int)material.Duration);
            }
			if(material.Type == TypeOfMaterial.Quiz.ToString() || material.Type == TypeOfMaterial.Assignment.ToString())
			{
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.QUIZ, 1);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.QUIZ_TIME, 1);
            }
            if (courseLearner.TotalTime > course.CourseStatistic.TotalTime)
            {
                courseLearner.TotalTime = course.CourseStatistic.TotalTime;
            }
            await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.MATERIAL, 1);
            await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.MATERIAL_TIME, 1);
            
			int maxIndex = lesson.LessonMaterials.Count - 1;
            string newLessonId = request.Info.LessonId;
			string newMaterialId = request.Info.MaterialId;
            LessonMaterial? temp = lesson.LessonMaterials.FirstOrDefault(m => m.MaterialId == request.Info.MaterialId);
            var newLesson = course.Lessons!.Where(l => l.Index == lesson.Index + 1).FirstOrDefault();
            if (temp != null && temp.Index == maxIndex)
            {
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.STAGE, 1);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.STAGE_TIME, 1);
				if(newLesson != null)
				{
                    newLessonId = newLesson.Id;
					newMaterialId = newLesson.LessonMaterials.FirstOrDefault(l => l.Index == 0).MaterialId;

                }
				else
				{
					await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.COURSE, 1);
                    await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.COURSE_TIME, 1);
                }

            }else if(temp != null && temp.Index < maxIndex)
			{
				newMaterialId = lesson.LessonMaterials.Where(l => l.Index == temp.Index + 1).FirstOrDefault()?.MaterialId;
			}
            courseLearner.CurrentLessonId = newLessonId;
            courseLearner.CurrentMaterialId = newMaterialId;

			courseLearner.ProgressPercentage = Math.Round((await _lessonRepository.CalculateMaterialProgressAsync(request.Info.LessonId, request.Info.MaterialId, (double)course.CourseStatistic.TotalTime)) * 100, 2);
            if (courseLearner.ProgressPercentage > 100)
            {
                courseLearner.ProgressPercentage = 100;
            }

            //await mediator.Send(new UpdateUsersStreakCommand(userMeta.UserId));
            if (userMeta.LastLearningDay == null)
            {
                userMeta.LastLearningDay = DateTime.UtcNow.ToUniversalTime();
            }

            DateTime lastLearningDay = userMeta.LastLearningDay.Value.Date;

            userMeta.CurrentStreak = (lastLearningDay == DateTime.UtcNow.Date.AddDays(-1)) ? userMeta.CurrentStreak + 1 : 1;
            userMeta.LastLearningDay = DateTime.UtcNow.ToUniversalTime();
            userMeta.LongestStreak = Math.Max((byte)userMeta.LongestStreak!, (byte)userMeta.CurrentStreak!);


            await _courseRepository.Update(course);
			await _userMetaRepository.Update(userMeta);

            
           
            var result = await _unitOfWork.SaveChangesAsync() > 0;
			return new APIResponse
			{
				IsError = !result,
				Payload = result ? courseLearner : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.UpdateFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.UpdateSuccesfully : MessageCommon.UpdateFailed,
					values = new Dictionary<string, string> { { "name", "user progess" } }
				}
			};

		}
	}
}
