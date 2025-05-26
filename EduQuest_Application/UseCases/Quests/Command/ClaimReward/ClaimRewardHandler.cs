using AutoMapper;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using Google.Rpc;
using MediatR;
using Stripe;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.QuestEnum;

namespace EduQuest_Application.UseCases.Quests.Command.ClaimReward;

public class ClaimRewardHandler : IRequestHandler<ClaimRewardCommand, APIResponse>
{
    private readonly IUserQuestRepository _userQuestRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILevelRepository _levelRepository;
    private const string key = "name";
    private const string value = "user quest";

    public ClaimRewardHandler(IUserQuestRepository userQuestRepository, IMapper mapper, IUserRepository userRepository,
        ICouponRepository couponRepository, IUnitOfWork unitOfWork, ILevelRepository levelRepository)
    {
        _userQuestRepository = userQuestRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _couponRepository = couponRepository;
        _unitOfWork = unitOfWork;
        _levelRepository = levelRepository;
    }

    public async Task<APIResponse> Handle(ClaimRewardCommand request, CancellationToken cancellationToken)
    {
        UserQuest? userQuest = await _userQuestRepository.GetById(request.UserQuestId);
        if (userQuest == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound,
                MessageCommon.NotFound, key, value);
        }
        ClaimRewardResponse response = new ClaimRewardResponse();
        string coupon;
        do
        {
            coupon = CodeGenerator.GenerateRandomCouponCode();
        } while (await _couponRepository.ExistByCode(coupon));
        var user = await _userRepository.GetById(request.UserId);

        if (user == null || user.UserMeta == null)
        {
            throw new Exception("User not found or UserMeta is null.");
        }
        if (userQuest.IsCompleted && !userQuest.IsRewardClaimed)
        {
            int[] rewardType = GetRewardType(userQuest.RewardTypes!);
            for (int i = 0; i < rewardType.Length; i++)
            {
                await HandleReward(rewardType[i], user, userQuest.RewardValues!.Split(','),
                    i, coupon, response, userQuest.Title!);
                userQuest.IsRewardClaimed = true;
                userQuest.CompleteDate = DateTime.Now.ToUniversalTime();
                await _userQuestRepository.Update(userQuest);
            }
            await _unitOfWork.SaveChangesAsync();
            var temp = _mapper.Map<ClaimRewardResponse>(user);
            response.mascotItem = temp.mascotItem;
            response.equippedItems = temp.equippedItems;
            response.statistic = temp.statistic;
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.Complete, response, key, value);
        }
        return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.AlreadyExists,
            MessageError.QuestAlreadyClaimed, key, value + $" with id: {request.UserQuestId}");
    }
    private int[] GetRewardType(string input)
    {
        int[] result = input.Split(',').Select(int.Parse).ToArray();
        return result;
    }
    private async Task HandleReward(int rewardType, User user, string[] rewardValue,
    int arrayIndex, string couponCode, ClaimRewardResponse response, string questName)
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
                    response.GoldAdded = BoostValue != null ? Convert.ToInt32(addedGold * BoostValue / 100) : addedGold;
                }
                break;

            case (int)RewardType.Exp:
                if (int.TryParse(rewardValue[arrayIndex], out int addedExp))
                {
                    user.UserMeta.Exp += BoostValue != null ? Convert.ToInt32(addedExp * BoostValue / 100) : addedExp;
                    response.ExpAdded = BoostValue != null ? Convert.ToInt32(addedExp * BoostValue / 100) : addedExp;
                    await HandlerLevelUp(user, response);
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
                EduQuest_Domain.Entities.Coupon coupon = new EduQuest_Domain.Entities.Coupon
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = now.ToUniversalTime(),
                    Discount = decimal.TryParse(rewardValue[arrayIndex], out decimal discount) ? discount : 0,
                    Description = $"{discount}% coupon for comlete quest {questName}.",
                    Code = couponCode,
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
                    response.BoosterAdded = Convert.ToInt32(booster);
                }
                break;

            default:
                break;
        }
    }

    //level handler
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
    private async Task HandlerLevelUp(User user, ClaimRewardResponse response)
    {
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
    }
}
