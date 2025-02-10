using AutoMapper;
using EduQuest_Application.DTO.Request.ShopItem;
using EduQuest_Application.DTO.Response.Mascot;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Mascot.Commands
{
    public class PurchaseMascotItemCommandHandler : IRequestHandler<PurchaseMascotItemCommand, APIResponse>
    {
        private readonly IShopItemRepository _shopItemRepository;
        private readonly IMascotInventoryRepository _mascotInventoryRepository;
        private readonly IUserStatisticRepository _userStatisticRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseMascotItemCommandHandler(
            IShopItemRepository shopItemRepository,
            IMascotInventoryRepository mascotInventoryRepository,
            IUserStatisticRepository userStatisticRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _shopItemRepository = shopItemRepository;
            _mascotInventoryRepository = mascotInventoryRepository;
            _userStatisticRepository = userStatisticRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(PurchaseMascotItemCommand request, CancellationToken cancellationToken)
        {
            // Check if the item exists in the shop
            var shopItem = await _shopItemRepository.GetById(request.ShopItemId);
            if (shopItem == null)
            {
                return new APIResponse
                {
                    IsError = false,
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
            var existingItem = await _mascotInventoryRepository.GetByUserIdAndItemIdAsync(request.UserId, request.ShopItemId);
            if (existingItem != null)
            {
                return new APIResponse
                {
                    IsError = false,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.Found,
                        Message = MessageCommon.NotFound,
                    },
                    Payload = null,
                    Message = null
                };
            }

            // Add the item to the user's inventory
            var mascotInventory = new MascotInventory
            {
                Id = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                ShopItemId = shopItem.Id,
                IsEquipped = false
            };

            //var userdetail = await _userStatisticRepository.GetById(request.UserId);
            //userdetail.Gold -= (int)shopItem.Price;

            
            await _mascotInventoryRepository.Add(mascotInventory);
            //await _userStatisticRepository.Update(userdetail);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<UserMascotDto>(mascotInventory);
            return new APIResponse
            {
                IsError = false,
                Errors = null,
                Payload = result,
                Message = null
            };
        }
    }
}
