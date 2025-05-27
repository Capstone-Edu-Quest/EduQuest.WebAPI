using AutoMapper;
using EduQuest_Application.DTO;
using EduQuest_Application.DTO.Response.ShopItems;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.ShopItem.Queries;

public class GetShopItemsQueryHandler : IRequestHandler<GetShopItemsQuery, APIResponse>
{
    private readonly IShopItemRepository _shopItemRepository;
    private readonly IMascotInventoryRepository _mascotInventoryRepository;
    private readonly IMapper _mapper;

    public GetShopItemsQueryHandler(
        IShopItemRepository shopItemRepository,
        IMascotInventoryRepository mascotInventoryRepository,
        IMapper mapper)
    {
        _shopItemRepository = shopItemRepository;
        _mascotInventoryRepository = mascotInventoryRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetShopItemsQuery request, CancellationToken cancellationToken)
    {
        var shopItems = await _shopItemRepository.GetAllItemAsync();

        var ownedItems = await _mascotInventoryRepository
            .GetItemsByUserIdAsync(request.UserId);

        var ownedItemIds = ownedItems.Select(i => i.ShopItemId).ToHashSet();

        var shopItemDtos = shopItems.Select(item => new ShopItemResponseDto
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            TagId = item.TagId != null ? item.TagId : null,
            TagName = item.Tag != null? item.Tag.Name : null,
            IsOwned = ownedItemIds.Contains(item.Id) 
        }).ToList();

        return new APIResponse
        {
            IsError = false,
            Payload = shopItemDtos,
            Message = null
        };
    }
}
