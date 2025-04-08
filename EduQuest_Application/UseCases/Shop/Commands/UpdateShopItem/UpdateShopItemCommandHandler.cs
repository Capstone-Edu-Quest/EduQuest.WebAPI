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
    private readonly IUnitOfWork _unitOfWork;

    public UpdateShopItemCommandHandler(IShopItemRepository shopItemRepository, IUnitOfWork unitOfWork)
    {
        _shopItemRepository = shopItemRepository;
        _unitOfWork = unitOfWork;
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

        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.UpdateSuccesfully, null, "name", "item");
    }
}
