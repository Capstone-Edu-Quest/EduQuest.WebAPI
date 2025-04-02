using AutoMapper;
using EduQuest_Application.DTO.Response.ShopItems;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;

namespace EduQuest_Application.UseCases.Shop.Commands;

public class CreateShopItemCommandHandler : IRequestHandler<CreateShopItemCommand, APIResponse>
{
    private readonly IShopItemRepository _shopItemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateShopItemCommandHandler(IShopItemRepository shopItemRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _shopItemRepository = shopItemRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(CreateShopItemCommand request, CancellationToken cancellationToken)
    {
        var newShopItems = request.ShopItems.Select(item => new EduQuest_Domain.Entities.ShopItem
        {
            Id = Guid.NewGuid().ToString(),
            Name = item.Id,
            Price = item.Price
        }).ToList();

        await _shopItemRepository.CreateRangeAsync(newShopItems);
        await _unitOfWork.SaveChangesAsync();

        var shopItemDto = _mapper.Map<IEnumerable<ShopItemResponseDto>>(newShopItems);

        return new APIResponse
        {
            IsError = false,
            Errors = null,
            Payload = shopItemDto,
            Message = null
        };
    }
}
