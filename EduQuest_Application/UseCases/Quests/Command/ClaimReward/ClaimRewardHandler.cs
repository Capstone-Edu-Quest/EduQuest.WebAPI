using AutoMapper;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using Google.Api;
using MediatR;
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
    private const string key = "name";
    private const string value = "user quest";

    public ClaimRewardHandler(IUserQuestRepository userQuestRepository, IMapper mapper, IUserRepository userRepository, 
        ICouponRepository couponRepository, IUnitOfWork unitOfWork)
    {
        _userQuestRepository = userQuestRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _couponRepository = couponRepository;
        _unitOfWork = unitOfWork;
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
        string coupon = CodeGenerator.GenerateRandomCouponCode();
        if(userQuest.IsCompleted && !userQuest.IsRewardClaimed)
        {
            int[] rewardType = GetRewardType(userQuest.RewardTypes!);
            for(int i = 0; i < rewardType.Length; i++)
            {
                await HandleReward(rewardType[i], request.UserId, userQuest.RewardValues!.Split(','),
                    i, coupon, response, userQuest.Title!);
                userQuest.IsRewardClaimed = true;
                userQuest.CompleteDate = DateTime.Now.ToUniversalTime();
                await _userQuestRepository.Update(userQuest);
            }
            await _unitOfWork.SaveChangesAsync();
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
    private async Task HandleReward(int rewardType, string userId, string[] rewardValue,
    int arrayIndex, string couponCode, ClaimRewardResponse response, string questName)
    {
        DateTime now = DateTime.Now;
        var user = await _userRepository.GetById(userId);

        if (user == null || user.UserMeta == null)
        {
            throw new Exception("User not found or UserMeta is null.");
        }

        if (arrayIndex < 0 || arrayIndex >= rewardValue.Length)
        {
            throw new IndexOutOfRangeException("Invalid array index.");
        }
        double? BoostValue = user.Boosters
            .Where(b => b.DueDate >= now)
            .OrderByDescending(b => b.BoostValue)
            .FirstOrDefault().BoostValue;
        switch (rewardType)
        {
            case (int)RewardType.Gold:
                if (int.TryParse(rewardValue[arrayIndex], out int addedGold))
                {
                    user.UserMeta.Gold += BoostValue != null ? Convert.ToInt32(addedGold * BoostValue / 100) : addedGold;
                    response.Gold = BoostValue != null ? Convert.ToInt32(addedGold * BoostValue / 100) : addedGold;
                }
                break;

            case (int)RewardType.Exp:
                if (int.TryParse(rewardValue[arrayIndex], out int addedExp))
                {
                    user.UserMeta.Exp += BoostValue != null ? Convert.ToInt32(addedExp * BoostValue / 100) : addedExp;
                    response.Exp = BoostValue != null ? Convert.ToInt32(addedExp * BoostValue / 100) : addedExp;
                }
                break;

            case (int)RewardType.Item:
                
                user.MascotItem.Add(new Mascot
                {
                    UserId = userId,
                    ShopItemId = rewardValue[arrayIndex],
                    CreatedAt = now.ToUniversalTime(),
                    IsEquipped = false,
                });
                response.Item = rewardValue[arrayIndex];
                break;

            case (int)RewardType.Coupon:
                Coupon coupon = new Coupon
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
                    CreatedBy = userId,
                };
                response.Coupon = couponCode;
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
                    response.Booster = Convert.ToInt32(booster);
                }
                break;

            default:
                break;
        }
    }
}
