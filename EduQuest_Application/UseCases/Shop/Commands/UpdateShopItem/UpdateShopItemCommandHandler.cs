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
        var existingShopItem = await _shopItemRepository.GetItemByName(request.Id);

        if (existingShopItem == null)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.NotFound, Constants.MessageCommon.NotFound, "name", request.Id);
        }

        // Cập nhật giá của món hàng
        existingShopItem.Price = request.Price;
        existingShopItem.UpdatedAt = DateTime.Now.ToUniversalTime();

        await _shopItemRepository.Update(existingShopItem);
        await _unitOfWork.SaveChangesAsync();

        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.UpdateSuccesfully, null, "name", request.Id);
    }
}
