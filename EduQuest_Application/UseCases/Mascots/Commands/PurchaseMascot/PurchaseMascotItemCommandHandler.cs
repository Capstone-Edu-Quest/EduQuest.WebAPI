using AutoMapper;
using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Application.DTO.Response.Mascot;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Notification;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Mascots.Commands.PurchaseMascot;

public class PurchaseMascotItemCommandHandler : IRequestHandler<PurchaseMascotItemCommand, APIResponse>
{
    private readonly IShopItemRepository _shopItemRepository;
    private readonly IMascotInventoryRepository _mascotInventoryRepository;
    private readonly IUserMetaRepository _userStatisticRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFireBaseRealtimeService _notifcation;
    private readonly IItemShardRepository _itemShardRepository;

    public PurchaseMascotItemCommandHandler(IShopItemRepository shopItemRepository, IMascotInventoryRepository mascotInventoryRepository, 
        IUserMetaRepository userStatisticRepository, IMapper mapper, IUnitOfWork unitOfWork, 
        IFireBaseRealtimeService notifcation, IItemShardRepository itemShardRepository)
    {
        _shopItemRepository = shopItemRepository;
        _mascotInventoryRepository = mascotInventoryRepository;
        _userStatisticRepository = userStatisticRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _notifcation = notifcation;
        _itemShardRepository = itemShardRepository;
    }

    public async Task<APIResponse> Handle(PurchaseMascotItemCommand request, CancellationToken cancellationToken)
    {
        // Check if the item exists in the shop
        var shopItem = await _shopItemRepository.GetItemByName(request.Name);
        if (shopItem == null)
        {
            return new APIResponse
            {
                IsError = true,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                },
                Payload = null,
                Message = null
            };
        }

        // Check if the user already owns the item
        var existingItem = await _mascotInventoryRepository.GetByUserIdAndItemIdAsync(request.UserId, shopItem.Id);
        if (existingItem != null)
        {
            return new APIResponse
            {
                IsError = true,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Message = MessageCommon.NotFound,
                },
                Payload = null,
                Message = new MessageResponse{ content = MessageCommon.AlreadyOwnThisItem }
            };
        }

        // Add the item to the user's inventory
        var mascotInventory = new EduQuest_Domain.Entities.Mascot
        {
            Id = Guid.NewGuid().ToString(),
            UserId = request.UserId,
            ShopItemId = shopItem.Name,
            IsEquipped = false
        };
        var userdetail = await _userStatisticRepository.GetByUserId(request.UserId);

        if (shopItem.Tag == null)
        {
            
            if (userdetail.Gold < shopItem.Price)
            {
                /*await _notifcation.PushNotificationAsync(
                    new NotificationDto
                    {
                        userId = request.UserId,
                        Content = NotificationMessage.NOT_ENOUGH_GOLD,
                        Receiver = request.UserId,
                        Url = BaseUrl.ShopItemUrl,
                    }
                 );*/
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotEnoughGold, MessageCommon.NotEnoughGold, "name", "item");
            }
            userdetail.Gold -= (int)shopItem.Price;
            /*await _notifcation.PushNotificationAsync(
                    new NotificationDto
                    {
                        userId = request.UserId,
                        Content = NotificationMessage.PURCHASE_ITEM_SUCCESSFULLY,
                        Receiver = request.UserId,
                        Url = BaseUrl.ShopItemUrl,
                        Values = new Dictionary<string, string>
                        {
                            { "item", shopItem.Name }
                        }
                    }
                 );*/

            await _mascotInventoryRepository.Add(mascotInventory);
            await _userStatisticRepository.Update(userdetail);
            var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            mascotInventory.User = new User
            {
                UserMeta = userdetail,
                ItemShards = await _itemShardRepository.GetItemShardsByUserId(request.UserId) 
            };

            var result = _mapper.Map<UserMascotDto>(mascotInventory);
            return new APIResponse
            {
                IsError = !saveResult,
                Payload = saveResult ? result : null,
                Errors = saveResult ? null : new ErrorResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = MessageCommon.SavingFailed,
                },
                Message = new MessageResponse
                {
                    content = saveResult ? MessageCommon.PurchaseItemSuccessfully : MessageCommon.SavingFailed
                }
            };
        }
        else
        {
            var tag = shopItem.Tag!;
            var shards = await _itemShardRepository.GetItemShardsByTagId(tag.Id, request.UserId);
            int price = (int)shopItem.Price;
            if(shards == null || shards.Quantity < price)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotEnoughGold, MessageCommon.NotEnoughGold, "name", "item");
            }
            shards.Quantity -= price;
            await _mascotInventoryRepository.Add(mascotInventory);
            await _itemShardRepository.Update(shards);
            var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            mascotInventory.User = new User
            {
                UserMeta = userdetail,
                ItemShards = await _itemShardRepository.GetItemShardsByUserId(request.UserId)
            };

            var result = _mapper.Map<UserMascotDto>(mascotInventory);
            return new APIResponse
            {
                IsError = !saveResult,
                Payload = saveResult ? result : null,
                Errors = saveResult ? null : new ErrorResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = MessageCommon.SavingFailed,
                },
                Message = new MessageResponse
                {
                    content = saveResult ? MessageCommon.PurchaseItemSuccessfully : MessageCommon.SavingFailed
                }
            };
        }
    }
}
