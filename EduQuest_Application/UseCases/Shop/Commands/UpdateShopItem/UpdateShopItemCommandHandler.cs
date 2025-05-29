using AutoMapper;
using EduQuest_Application.DTO.Request.ShopItem;
using EduQuest_Application.DTO.Response.ShopItems;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;

namespace EduQuest_Application.UseCases.Shop.Commands.UpdateShopItem;

public class UpdateShopItemCommandHandler : IRequestHandler<UpdateShopItemCommand, APIResponse>
{
    private readonly IShopItemRepository _shopItemRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateShopItemCommandHandler(IShopItemRepository shopItemRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _shopItemRepository = shopItemRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(UpdateShopItemCommand request, CancellationToken cancellationToken)
    {
        //if (!request.items.Any())
        //{
        //    return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.NotFound, Constants.MessageCommon.NotFound, "name", "item");
        //}

        await _shopItemRepository.DeleteAllAsync();

        var itemNames = request.items.Select(i => i.Name).ToList();

        var existingItems = await _shopItemRepository.GetByNamesAsync(itemNames);

        var newShopItems = request.items
            .Where(item => !existingItems.Any(existingItem => existingItem.Name == item.Name))
            .Select(item => new EduQuest_Domain.Entities.ShopItem
            {
                Id = Guid.NewGuid().ToString(),
                Name = item.Name,
                Price = item.Price,
                TagId = item.TagId != null ? item.TagId : null
            }).ToList();

        if (newShopItems.Any())
        {
            await _shopItemRepository.CreateRangeAsync(newShopItems);
        }

        await _unitOfWork.SaveChangesAsync();

        foreach (var item in request.items)
        {
            var existingItem = existingItems.FirstOrDefault(e => e.Name == item.Name);
            if (existingItem != null)
            {
                existingItem.Price = item.Price; 
                await _shopItemRepository.UpdateShopItems(existingItem.Name, existingItem.Price);
            }
        }


        var result = await _shopItemRepository.GetAllItemAsync();

        var mapResult = _mapper.Map<List<ShopItemFilterResponseDto>>(result);

        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.UpdateSuccesfully, mapResult, "name", "item");
    }
}


