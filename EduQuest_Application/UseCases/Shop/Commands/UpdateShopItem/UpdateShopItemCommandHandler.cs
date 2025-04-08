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

    public UpdateShopItemCommandHandler(IShopItemRepository shopItemRepository, IMapper mapper)
    {
        _shopItemRepository = shopItemRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(UpdateShopItemCommand request, CancellationToken cancellationToken)
    {
        if (!request.items.Any())
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.NotFound, Constants.MessageCommon.NotFound, "name", "item");
        }

        foreach (var item in request.items)
        {
            await _shopItemRepository.UpdateShopItems(item.Name, item.Price);
        }

        var result = await _shopItemRepository.GetAllItemAsync();
        var mapResult = _mapper.Map<List<ShopItemFilterResponseDto>>(result);

        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.UpdateSuccesfully, mapResult, "name", "item");
    }
}
