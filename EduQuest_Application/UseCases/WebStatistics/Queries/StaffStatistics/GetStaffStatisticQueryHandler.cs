using EduQuest_Application.Helper;
using EduQuest_Domain.Models.PlatformStatisticDashBoard;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.WebStatistics.Queries.StaffStatistics;

public class GetStaffStatisticQueryHandler : IRequestHandler<GetStaffStatisticQuery, APIResponse>
{
    private readonly ILevelRepository levelRepository;
    private readonly ICouponRepository couponRepository;
    private readonly IShopItemRepository shopItemRepository;
    private readonly IQuestRepository questRepository;

    public GetStaffStatisticQueryHandler(ILevelRepository levelRepository, ICouponRepository couponRepository, IShopItemRepository shopItemRepository, IQuestRepository questRepository)
    {
        this.levelRepository = levelRepository;
        this.couponRepository = couponRepository;
        this.shopItemRepository = shopItemRepository;
        this.questRepository = questRepository;
    }

    public async Task<APIResponse> Handle(GetStaffStatisticQuery request, CancellationToken cancellationToken)
    {
        var response = new PlatformStatisticResponseDto();
        response.ShopItems = await shopItemRepository.GetShopItemStatisticsDto();
        response.Coupons = await couponRepository.CouponStatistics();
        response.LevelExp = await levelRepository.GetLevelExpStatistic();
        response.Quests = await questRepository.GetQuestStatistic();


        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.Complete,
            response, "name", "dashboard");
    }
}
