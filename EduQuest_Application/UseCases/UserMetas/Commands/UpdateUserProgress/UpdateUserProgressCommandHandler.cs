using AutoMapper;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
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
        private readonly ILessonContentRepository _lessonContentRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
        private readonly IUserQuestRepository _userQuestRepository;
        private readonly IRedisCaching _redis;
        private readonly IStudyTimeRepository _studyTimeRepository;
        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        private readonly IItemShardRepository _itemShardRepository;
        private readonly ICouponRepository _couponRepository;
		private readonly IQuizRepository _quizRepository;
		private readonly IAssignmentRepository _assignmentRepository;
		private readonly ILevelRepository _levelRepository;

		public UpdateUserProgressCommandHandler(IUserMetaRepository userMetaRepository, IUnitOfWork unitOfWork, ICourseRepository courseRepository, ILessonRepository lessonRepository, ILessonContentRepository lessonContentRepository, IMaterialRepository materialRepository, ISystemConfigRepository systemConfigRepository, IUserQuestRepository userQuestRepository, IRedisCaching redis, IStudyTimeRepository studyTimeRepository, IMediator mediator, IMapper mapper, IItemShardRepository itemShardRepository, ICouponRepository couponRepository, IQuizRepository quizRepository, IAssignmentRepository assignmentRepository, ILevelRepository levelRepository)
		{
			_userMetaRepository = userMetaRepository;
			_unitOfWork = unitOfWork;
			_courseRepository = courseRepository;
			_lessonRepository = lessonRepository;
			_lessonContentRepository = lessonContentRepository;
			_materialRepository = materialRepository;
			_systemConfigRepository = systemConfigRepository;
			_userQuestRepository = userQuestRepository;
			_redis = redis;
			_studyTimeRepository = studyTimeRepository;
			this.mediator = mediator;
			_mapper = mapper;
			_itemShardRepository = itemShardRepository;
			_couponRepository = couponRepository;
			_quizRepository = quizRepository;
			_assignmentRepository = assignmentRepository;
			_levelRepository = levelRepository;
		}

		public async Task<APIResponse> Handle(UpdateUserProgressCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;
            var userMeta = await _userMetaRepository.GetByUserId(request.UserId);
            var user = userMeta.User;
			//Get lesson content
			var type = await _lessonContentRepository.GetMaterialTypeByIdAsync(request.Info.ContentId);
            object content = null;
			switch (type)
			{
				case GeneralEnums.TypeOfMaterial.Document:
				case GeneralEnums.TypeOfMaterial.Video:
					content = await _materialRepository.GetById(request.Info.ContentId);
                    break;
				
				case GeneralEnums.TypeOfMaterial.Quiz:
					content = await _quizRepository.GetById(request.Info.ContentId);
                    break;
				
				case GeneralEnums.TypeOfMaterial.Assignment:
					content = await _assignmentRepository.GetById(request.Info.ContentId);
					break;
			
			}

			//var material = await _materialRepository.GetMataterialQuizAssById(request.Info.ContentId);
			//Get lesson
			var lesson = await _lessonRepository.GetById(request.Info.LessonId);

            //Get CourseLeaner
            var course = await _courseRepository.GetById(lesson.CourseId);
            var courseLearner = course.CourseLearners.FirstOrDefault(x => x.UserId == request.UserId);
            if (courseLearner == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageLearner.NotLearner, $"Not Found", "name", $"Not Found in Course ID {lesson.CourseId}");
            }
            if (courseLearner.ProgressPercentage >= 100 || courseLearner.TotalTime.Value >= course.CourseStatistic.TotalTime.Value)
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
            var totalMaterial = await _lessonContentRepository.GetTotalContent(course.Id);
            courseLearner.ProgressPercentage = Math.Round((await _lessonRepository.CalculateContentProgressAsync(request.Info.LessonId, request.Info.ContentId, totalMaterial)) * 100, 2);
            if (courseLearner.ProgressPercentage > 100)
            {
                courseLearner.ProgressPercentage = 100;
            }
            if (userMeta.LastLearningDay == null)
            {
                userMeta.LastLearningDay = DateTime.Now.ToUniversalTime();
            }

            DateTime lastLearningDay = userMeta.LastLearningDay.Value.Date;

            userMeta.CurrentStreak = (lastLearningDay == DateTime.UtcNow.Date.AddDays(-1)) ? userMeta.CurrentStreak + 1 : 1;
            userMeta.LastLearningDay = DateTime.Now.ToUniversalTime();
            userMeta.LongestStreak = Math.Max((byte)userMeta.LongestStreak!, (byte)userMeta.CurrentStreak!);
            

            if (request.Info.Time != null)
            {

				if (content is Material material1)
				{
					courseLearner.TotalTime += material1.Duration;
				}
				else if (content is Quiz quiz )
				{
					courseLearner.TotalTime += quiz.TimeLimit;
				}
				else if (content is Assignment assignment)
				{
					courseLearner.TotalTime += assignment.TimeLimit;
				}

				userMeta.TotalStudyTime += request.Info.Time;
                int addedExp = GeneralHelper.GenerateExpEarned(request.Info.Time);
                userMeta.Exp += addedExp;
                LevelUpNotiModel levelup = await HandlerLevelUp(user, new ClaimRewardResponse());
                levelup.ExpAdded = addedExp;
                response.LevelInfo = levelup;
                await _redis.AddToSortedSetAsync("leaderboard:season1", request.UserId, userMeta.TotalStudyTime.Value);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME, request.Info.Time.Value);
                await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME_TIME, request.Info.Time.Value);
            }
            else
            {
                int addedExp = 0;
				if (content is Material material1)
				{
					courseLearner.TotalTime += material1.Duration;
					userMeta.TotalStudyTime += material1.Duration;
					addedExp = GeneralHelper.GenerateExpEarned(material1.Duration);
					// Sử dụng addedExp tiếp theo ở đây
				}
				else if (content is Quiz quiz)
				{
					courseLearner.TotalTime += quiz.TimeLimit;
					userMeta.TotalStudyTime += quiz.TimeLimit;
					addedExp = GeneralHelper.GenerateExpEarned(quiz.TimeLimit);
					// Sử dụng addedExp tiếp theo ở đây
				}
				else if (content is Assignment assignment)
				{
					courseLearner.TotalTime += assignment.TimeLimit;
					userMeta.TotalStudyTime += assignment.TimeLimit;
					addedExp = GeneralHelper.GenerateExpEarned(assignment.TimeLimit);
					// Sử dụng addedExp tiếp theo ở đây
				}

				userMeta.Exp += addedExp;
                LevelUpNotiModel levelup = await HandlerLevelUp(user, new ClaimRewardResponse());
                levelup.ExpAdded = addedExp;
                response.LevelInfo = levelup;
                await _redis.AddToSortedSetAsync("leaderboard:season1", request.UserId, userMeta.TotalStudyTime.Value);
				int durationTime = 0;

				if (content is Material material2)
				{
					durationTime = (int)material2.Duration;
				}
				else if (content is Quiz quiz)
				{
					durationTime = (int)quiz.TimeLimit;
				}
				else if (content is Assignment assignment)
				{
					durationTime = (int)assignment.TimeLimit;
				}

				// Gọi update với durationTime đã xác định
				await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME, durationTime);
				await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME_TIME, durationTime);


				//await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME, (int)material.Duration);
    //            await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME_TIME, (int)material.Duration);
            }
            if (content is Quiz quiz1 || content is Assignment assignment1)
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
			double times;

			if (content is Material material)
			{
				times = material.Duration ?? 0; // Nếu Duration có thể null thì dùng ?? 0 để tránh lỗi
			}
			else if (content is Quiz quiz)
			{
				times = quiz.TimeLimit ?? 0;
			}
			else if (content is Assignment assignment)
			{
				times = assignment.TimeLimit ?? 0;
			}
			else
			{
				times = 0; // Trường hợp khác hoặc content null
			}

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
            courseLearner.UpdatedAt = DateTime.Now.ToUniversalTime();
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

        #region handle level up
        private int[] GetRewardType(string input)
        {
            int[] result = input.Split(',').Select(int.Parse).ToArray();
            return result;
        }
        private async Task HandleReward(int rewardType, User user, string[] rewardValue, int arrayIndex,
       ClaimRewardResponse response)
        {
            DateTime now = DateTime.Now;


            if (arrayIndex < 0 || arrayIndex >= rewardValue.Length)
            {
                throw new IndexOutOfRangeException("Invalid array index.");
            }
            double? BoostValue = null;
            if (user.Boosters != null)
            {
                var booster = user.Boosters
               .Where(b => b.DueDate >= now)
               .OrderByDescending(b => b.BoostValue)
               .FirstOrDefault();
                BoostValue = booster?.BoostValue;
            }
            switch (rewardType)
            {
                case (int)RewardType.Gold:
                    if (int.TryParse(rewardValue[arrayIndex], out int addedGold))
                    {
                        user.UserMeta.Gold += BoostValue != null ? Convert.ToInt32(addedGold * BoostValue / 100) : addedGold;
                    }
                    break;

                case (int)RewardType.Exp:
                    if (int.TryParse(rewardValue[arrayIndex], out int addedExp))
                    {
                        user.UserMeta.Exp += BoostValue != null ? Convert.ToInt32(addedExp * BoostValue / 100) : addedExp;
                    }
                    break;

                case (int)RewardType.Item:

                    if (user.MascotItem != null)
                    {
                        user.MascotItem.Add(new Mascot
                        {
                            UserId = user.Id,
                            ShopItemId = rewardValue[arrayIndex],
                            CreatedAt = now.ToUniversalTime(),
                            IsEquipped = false,
                        });
                    }
                    else
                    {
                        user.MascotItem = new List<Mascot> {new Mascot
                    {
                        UserId = user.Id,
                        ShopItemId = rewardValue[arrayIndex],
                        CreatedAt = now.ToUniversalTime(),
                        IsEquipped = false,
                    }
                };
                    }

                    break;

                case (int)RewardType.Coupon:
                    string code;
                    do
                    {
                        code = CodeGenerator.GenerateRandomCouponCode();
                    } while (await _couponRepository.ExistByCode(code));
                    EduQuest_Domain.Entities.Coupon coupon = new EduQuest_Domain.Entities.Coupon
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedAt = now.ToUniversalTime(),
                        Discount = decimal.TryParse(rewardValue[arrayIndex], out decimal discount) ? discount : 0,
                        Description = $"{discount}% coupon for level up.",
                        Code = code,
                        StartTime = now.ToUniversalTime(),
                        ExpireTime = now.AddDays(90).ToUniversalTime(),
                        AllowUsagePerUser = 1,
                        Limit = 1,
                        Usage = 0,
                        CreatedBy = user.Id,
                    };
                    response.Coupon.Add(_mapper.Map<RewardCoupon>(coupon));
                    await _couponRepository.Add(coupon);
                    break;

                case (int)RewardType.Booster:
                    if (double.TryParse(rewardValue[arrayIndex], out double booster))
                    {
                        user.Boosters.Add(new Booster
                        {
                            BoostValue = booster,
                            DueDate = now.AddDays(7).ToUniversalTime()
                        });
                    }
                    break;

                default:
                    break;
            }
        }
        private async Task<LevelUpNotiModel> HandlerLevelUp(User user, ClaimRewardResponse response)
        {
            LevelUpNotiModel levelUpNotiModel = new LevelUpNotiModel();
            var meta = user.UserMeta;
            var currentExp = meta.Exp;
            int maxLevel = await _levelRepository.GetMaxLevelNumber();
            while (currentExp > 1)
            {
                var currentLevel = await _levelRepository.GetByLevelNum(meta.Level.Value);
                if (currentLevel == null)
                {
                    meta.Level = maxLevel;
                    break;
                }
                if (currentExp < currentLevel.Exp)
                    break;

                if (currentLevel.Level < maxLevel)
                {
                    int[] rewardType = GetRewardType(currentLevel.RewardTypes!);
                    for (int i = 0; i < rewardType.Length; i++)
                    {
                        await HandleReward(rewardType[i], user, currentLevel.RewardValues!.Split(','), i, response);
                    }
                    meta.Level++;
                    meta.Exp -= currentLevel.Exp;
                    currentExp -= currentLevel.Exp;
                    levelUpNotiModel.NewLevel = meta.Level;
                    var level = await _levelRepository.GetByLevelNum(meta.Level.Value);
                    levelUpNotiModel.NewLevelMaxExp = level != null ? level.Exp : null;
                }
                else
                {
                    break;
                }

            }
            return levelUpNotiModel;
        }
        #endregion
    }
}
