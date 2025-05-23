using AutoMapper;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;
using static EduQuest_Domain.Enums.QuestEnum;

namespace EduQuest_Application.UseCases.UserMetas.Commands.UpdateUserProgress
{
    public class UpdateUserProgressCommandHandler : IRequestHandler<UpdateUserProgressCommand, APIResponse>
    {
        private readonly IUserMetaRepository _userMetaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ILessonContentRepository _lessonMaterialRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
        private readonly IUserQuestRepository _userQuestRepository;
        private readonly IRedisCaching _redis;
        private readonly IStudyTimeRepository _studyTimeRepository;
        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        private readonly IItemShardRepository _itemShardRepository;

        public UpdateUserProgressCommandHandler(IUserMetaRepository userMetaRepository, IUnitOfWork unitOfWork, ICourseRepository courseRepository, ILessonRepository lessonRepository, ILessonContentRepository lessonMaterialRepository, IMaterialRepository materialRepository, ISystemConfigRepository systemConfigRepository, IUserQuestRepository userQuestRepository,
            IRedisCaching redis, IStudyTimeRepository studyTimeRepository, IMediator mediator, IMapper mapper, IItemShardRepository itemShardRepository)
        {
            _userMetaRepository = userMetaRepository;
            _unitOfWork = unitOfWork;
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
            _lessonMaterialRepository = lessonMaterialRepository;
            _materialRepository = materialRepository;
            _systemConfigRepository = systemConfigRepository;
            _userQuestRepository = userQuestRepository;
            _redis = redis;
            _studyTimeRepository = studyTimeRepository;
            this.mediator = mediator;
            _mapper = mapper;
            _itemShardRepository = itemShardRepository;
        }

        public async Task<APIResponse> Handle(UpdateUserProgressCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;
            var userMeta = await _userMetaRepository.GetByUserId(request.UserId);
            //Get material
            var material = await _materialRepository.GetMataterialQuizAssById(request.Info.ContentId);
            //Get lesson
            var lesson = await _lessonRepository.GetById(request.Info.LessonId);

            //Get CourseLeaner
            var course = await _courseRepository.GetById(lesson.CourseId);
            var courseLearner = course.CourseLearners.FirstOrDefault(x => x.UserId == request.UserId);
            if (courseLearner == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageLearner.NotLearner, $"Not Found", "name", $"Not Found in Course ID {lesson.CourseId}");
            }
            if (courseLearner.TotalTime.Value >= course.CourseStatistic.TotalTime.Value)
            {
                return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully,
                    _mapper.Map<CourseLearnerResponse>(courseLearner), "name", "user progess");
            }


            var currentLesson = course.Lessons!.Where(l => l.Id == courseLearner.CurrentLessonId).FirstOrDefault();
            int maxIndex = lesson.LessonContents.Count - 1;
            string newLessonId = request.Info.LessonId;

            LessonContent? temp = lesson.LessonContents.FirstOrDefault(m => m.Index == courseLearner.CurrentContentIndex);
            int nextIndex = temp.Index;
            LessonContent? processingContent = lesson.LessonContents.FirstOrDefault(m => m.MaterialId == request.Info.ContentId || m.QuizId == request.Info.ContentId || m.AssignmentId == request.Info.ContentId);
            var newLesson = course.Lessons!.Where(l => l.Index == lesson.Index + 1).FirstOrDefault();

            var response = _mapper.Map<CourseLearnerResponse>(courseLearner);
            var tag = course.Tags.Where(l => l.Type == TagType.Subject.ToString()).FirstOrDefault();

            var processingMaterialLesson = course.Lessons!.Where(l => l.Id == processingContent.LessonId).FirstOrDefault();
            if (temp == null || temp.Index > processingContent.Index && temp.LessonId == processingContent.LessonId
                || currentLesson.Index > processingMaterialLesson.Index)//only happened when re learning courses materials when undon courses
            {
                return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, courseLearner, "name", "user progess");
            }

            if (temp != null && temp.Index == maxIndex)
            {
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.STAGE, 1);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.STAGE_TIME, 1);
                if (newLesson != null && currentLesson.Index < newLesson.Index)
                {
                    newLessonId = newLesson.Id;
                    nextIndex = 0;
                    //handle Item shards
                    int addedShards = GeneralHelper.GenerateItemShards(tag);
                    response.AddedItemShard = addedShards;
                    var item = await _itemShardRepository.GetItemShardsByTagId(tag!.Id, request.UserId);
                    if (item != null)
                    {
                        item.Quantity += addedShards;
                        item.UpdatedAt = DateTime.Now.ToUniversalTime();
                        await _itemShardRepository.Update(item);
                    }
                    else
                    {
                        await _itemShardRepository.Add(new ItemShards
                        {
                            UserId = request.UserId,
                            TagId = tag.Id,
                            Quantity = addedShards,
                            Id = Guid.NewGuid().ToString(),
                            CreatedAt = DateTime.Now.ToUniversalTime(),
                        });
                    }
                    //
                }
                if (newLesson == null)
                {
                    await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.COURSE, 1);
                    await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.COURSE_TIME, 1);
                    await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.STAGE, 1);
                    await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.STAGE_TIME, 1);
                    //handle Item shards
                    int addedShards = GeneralHelper.GenerateItemShards(tag);
                    response.AddedItemShard = addedShards;
                    var item = await _itemShardRepository.GetItemShardsByTagId(tag!.Id, request.UserId);
                    if (item != null)
                    {
                        item.Quantity += addedShards;
                        item.UpdatedAt = DateTime.Now.ToUniversalTime();
                        await _itemShardRepository.Update(item);
                    }
                    else
                    {
                        await _itemShardRepository.Add(new ItemShards
                        {
                            UserId = request.UserId,
                            TagId = tag.Id,
                            Quantity = addedShards,
                            Id = Guid.NewGuid().ToString(),
                            CreatedAt = DateTime.Now.ToUniversalTime(),
                        });
                    }
                    //
                }

            }
            else if (temp != null && temp.Index < maxIndex)
            {
                nextIndex += 1;
            }
            courseLearner.CurrentLessonId = newLessonId;
            courseLearner.CurrentContentIndex = nextIndex;
            var totalMaterial = await _lessonMaterialRepository.GetTotalContent(course.Id);
            courseLearner.ProgressPercentage = Math.Round((await _lessonRepository.CalculateContentProgressAsync(request.Info.LessonId, request.Info.ContentId, totalMaterial)) * 100, 2);
            if (courseLearner.ProgressPercentage > 100)
            {
                courseLearner.ProgressPercentage = 100;
            }
            if (userMeta.LastLearningDay == null)
            {
                userMeta.LastLearningDay = DateTime.UtcNow.ToUniversalTime();
            }

            DateTime lastLearningDay = userMeta.LastLearningDay.Value.Date;

            userMeta.CurrentStreak = (lastLearningDay == DateTime.UtcNow.Date.AddDays(-1)) ? userMeta.CurrentStreak + 1 : 1;
            userMeta.LastLearningDay = DateTime.UtcNow.ToUniversalTime();
            userMeta.LongestStreak = Math.Max((byte)userMeta.LongestStreak!, (byte)userMeta.CurrentStreak!);
            if (request.Info.Time != null)
            {
                courseLearner.TotalTime += material.Duration;
                userMeta.TotalStudyTime += request.Info.Time;
                await _redis.AddToSortedSetAsync("leaderboard:season1", request.UserId, userMeta.TotalStudyTime.Value);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME, request.Info.Time.Value);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME_TIME, request.Info.Time.Value);
            }
            else
            {
                courseLearner.TotalTime += material.Duration;
                userMeta.TotalStudyTime += material.Duration;
                await _redis.AddToSortedSetAsync("leaderboard:season1", request.UserId, userMeta.TotalStudyTime.Value);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME, (int)material.Duration);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME_TIME, (int)material.Duration);
            }
            if (material.Type == TypeOfMaterial.Quiz.ToString() || material.Type == TypeOfMaterial.Assignment.ToString())
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

            await _courseRepository.Update(course);
            await _userMetaRepository.Update(userMeta);
            var studyTime = await _studyTimeRepository.GetByDate(now, request.UserId);
            double times = request.Info.Time != null ? request.Info.Time.Value : material.Duration!.Value;
            if (studyTime != null)
            {
                studyTime.StudyTimes += times;
                await _studyTimeRepository.Update(studyTime);
            }
            else
            {
                await _studyTimeRepository.Add(new StudyTime
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = request.UserId,
                    StudyTimes = times,
                    Date = now.ToUniversalTime()
                });
            }

            var result = await _unitOfWork.SaveChangesAsync() > 0;

            var userItemShards = await _itemShardRepository.GetItemShardsByUserId(request.UserId);
            Dictionary<string, int> shards = new Dictionary<string, int>();
            if (userItemShards != null)
            {
                foreach (var shard in userItemShards)
                {
                    shards.Add(shard.Tags.Name, shard.Quantity);
                }
                response.ItemShards = shards;
            }

            return new APIResponse
            {
                IsError = !result,
                Payload = result ? response : null,
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
