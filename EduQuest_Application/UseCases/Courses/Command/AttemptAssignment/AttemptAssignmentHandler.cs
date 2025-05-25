using AutoMapper;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Quests;
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

namespace EduQuest_Application.UseCases.Courses.Command.AttemptAssignment;

public class AttemptAssignmentHandler : IRequestHandler<AttemptAssignmentCommand, APIResponse>
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly IAssignmentAttemptRepository _assignmentAttemptRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserQuestRepository _userQuestRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserMetaRepository _userMetaRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IRedisCaching _redis;
    private readonly IStudyTimeRepository _studyTimeRepository;
    private readonly ILessonContentRepository _lessonMaterialRepository;
    private readonly IMapper _mapper;
    private readonly IItemShardRepository _itemShardRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly ILevelRepository _levelRepository;

    public AttemptAssignmentHandler(IAssignmentRepository assignmentRepository,
        ILessonRepository lessonRepository,
        IAssignmentAttemptRepository assignmentAttemptRepository,
        IUnitOfWork unitOfWork,
        IUserQuestRepository userQuestRepository,
        ICourseRepository courseRepository,
        IUserMetaRepository userMetaRepository,
        IMaterialRepository materialRepository, IRedisCaching redis,
        IStudyTimeRepository studyTimeRepository, ILessonContentRepository lessonMaterialRepository, IMapper mapper,
        IItemShardRepository itemShardRepository, ICouponRepository couponRepository, ILevelRepository levelRepository)
    {
        _assignmentRepository = assignmentRepository;
        _lessonRepository = lessonRepository;
        _assignmentAttemptRepository = assignmentAttemptRepository;
        _unitOfWork = unitOfWork;
        _userQuestRepository = userQuestRepository;
        _courseRepository = courseRepository;
        _userMetaRepository = userMetaRepository;
        _materialRepository = materialRepository;
        _redis = redis;
        _studyTimeRepository = studyTimeRepository;
        _lessonMaterialRepository = lessonMaterialRepository;
        _mapper = mapper;
        _itemShardRepository = itemShardRepository;
        _couponRepository = couponRepository;
        _levelRepository = levelRepository;
    }

    public async Task<APIResponse> Handle(AttemptAssignmentCommand request, CancellationToken cancellationToken)
    {
        AssignmentAttemptResponseUser response = new AssignmentAttemptResponseUser();
        int attempNo = await _assignmentAttemptRepository.GetAttemptNo(request.Attempt.AssignmentId, request.LessonId, request.UserId) + 1;
        if (attempNo > 1)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.OK, MessageCommon.AlreadyExists,
                MessageCommon.AlreadyExists, "name", "assignemt attemp");
        }
        DateTime now = DateTime.Now;
        var lesson = await _lessonRepository.GetById(request.LessonId);

        if (lesson == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound,
                MessageCommon.NotFound, "name", "lesson");
        }
        var materials = await _materialRepository.GetMaterialsByIds(lesson.LessonContents.Select(x => x.MaterialId).ToList());
        //var material = materials.Where(m => m.AssignmentId == request.Attempt.AssignmentId).FirstOrDefault();
        var lessonMaterial = lesson.LessonContents.FirstOrDefault(m => m.AssignmentId == request.Attempt.AssignmentId);
        var assignment = await _assignmentRepository.GetById(request.Attempt.AssignmentId);
        if (assignment == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound,
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
        response = _mapper.Map<AssignmentAttemptResponseUser>(attempt);


        var course = await _courseRepository.GetById(lesson.CourseId);
        var learner = course.CourseLearners.FirstOrDefault(l => l.UserId == request.UserId);
        int maxIndex = lesson.LessonContents.Count - 1;
        string newLessonId = lesson.Id;
        var newLesson = course.Lessons!.Where(l => l.Index == lesson.Index + 1).FirstOrDefault();
        LessonContent? temp = lesson.LessonContents.FirstOrDefault(m => m.Index == learner.CurrentContentIndex);
        int nextIndex = temp.Index;
        LessonContent? processingMaterial = lesson.LessonContents.FirstOrDefault(m => m.AssignmentId == request.Attempt.AssignmentId);
        var tag = course.Tags.Where(l => l.Type == TagType.Subject.ToString()).FirstOrDefault();
        var currentLesson = course.Lessons!.Where(l => l.Id == learner.CurrentLessonId).FirstOrDefault();
        var processingMaterialLesson = course.Lessons!.Where(l => l.Id == processingMaterial.LessonId).FirstOrDefault();
        if (temp == null || temp.Index > processingMaterial.Index && temp.LessonId == processingMaterial.LessonId
            || currentLesson.Index > processingMaterialLesson.Index)//only happened when re learning courses materials when undon courses
        {
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, response, "name", "assignment");
        }


        if (lessonMaterial.Index == maxIndex && newLesson != null)
        {
            newLessonId = newLesson.Id;
            nextIndex = 0;
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
        if (newLesson == null && lessonMaterial.Index == maxIndex)
        {
            await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.STAGE, 1);
            await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.STAGE_TIME, 1);
            await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.COURSE, 1);
            await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.COURSE_TIME, 1);
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
        if (lessonMaterial.Index < maxIndex)
        {
            nextIndex += 1;
        }

        learner.CurrentLessonId = newLessonId;
        learner.CurrentContentIndex = nextIndex;
        var totalMaterial = await _lessonMaterialRepository.GetTotalContent(course.Id);
        learner.ProgressPercentage = Math.Round((await _lessonRepository.CalculateAssignmentProgressAsync(request.LessonId, request.Attempt.AssignmentId, totalMaterial)) * 100, 2);
        if (learner.ProgressPercentage > 100)
        {
            learner.ProgressPercentage = 100;
        }

        var userMeta = await _userMetaRepository.GetByUserId(request.UserId);
        userMeta.TotalStudyTime += request.Attempt.TotalTime;
        if (userMeta.LastLearningDay == null)
        {
            userMeta.LastLearningDay = DateTime.Now.ToUniversalTime();
        }

        DateTime lastLearningDay = userMeta.LastLearningDay.Value.Date;

        userMeta.CurrentStreak = (lastLearningDay == DateTime.UtcNow.Date.AddDays(-1)) ? userMeta.CurrentStreak + 1 : 1;
        userMeta.LastLearningDay = DateTime.Now.ToUniversalTime();
        userMeta.LongestStreak = Math.Max((byte)userMeta.LongestStreak!, (byte)userMeta.CurrentStreak!);
        learner.TotalTime += assignment.TimeLimit;
        if (learner.TotalTime > course.CourseStatistic.TotalTime)
        {
            learner.TotalTime = course.CourseStatistic.TotalTime;
        }

        await _userMetaRepository.Update(userMeta);
        await _courseRepository.Update(course);
        var studyTime = await _studyTimeRepository.GetByDate(now, request.UserId);
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
                StudyTimes = request.Attempt.TotalTime,
                Date = now.ToUniversalTime()
            });
        }
        await _redis.AddToSortedSetAsync("leaderboard:season1", request.UserId, userMeta.TotalStudyTime.Value);
        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.MATERIAL, 1);
        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.MATERIAL_TIME, 1);
        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.QUIZ, 1);
        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.QUIZ_TIME, 1);
        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME, request.Attempt.TotalTime);
        await _userQuestRepository.UpdateUserQuestsProgress(request.UserId, QuestType.LEARNING_TIME_TIME, request.Attempt.TotalTime);

        LevelUpNotiModel levelup = await HandlerLevelUp(userMeta.User, new ClaimRewardResponse());
        int addedExp = GeneralHelper.GenerateExpEarned(request.Attempt.TotalTime);
        userMeta.Exp += addedExp;
        levelup.ExpAdded = addedExp;
        await _unitOfWork.SaveChangesAsync();
        response.LevelInfo = levelup;

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

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.Complete,
            response, "name", "assignment");
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
            if (currentExp >= currentLevel.Exp)
            {
                if (currentLevel.Level <= maxLevel)
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
                }
                else
                {
                    break;
                }
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
