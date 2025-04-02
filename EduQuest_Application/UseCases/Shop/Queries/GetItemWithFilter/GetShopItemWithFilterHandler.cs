using AutoMapper;
using EduQuest_Application.DTO.Response.ShopItems;
using EduQuest_Application.UseCases.Shop.Queries.GetItemWithFilterl;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Shop.Queries.GetItemWithFilter;

public class GetShopItemWithFilterHandler : IRequestHandler<GetShopItemWithFilter, APIResponse>
{
    private readonly IShopItemRepository _shopItemRepository;
    private readonly IMapper mapper;

    public GetShopItemWithFilterHandler(IShopItemRepository shopItemRepository, IMapper mapper)
    {
        _shopItemRepository = shopItemRepository;
        this.mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetShopItemWithFilter request, CancellationToken cancellationToken)
    {
        var existItems = await _shopItemRepository.GetItemWithFilter(request.Name);
        var result = mapper.Map<IEnumerable<ShopItemFilterResponseDto>>(existItems);
        return Helper.GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, result, "name", "shop items");
    }
}
