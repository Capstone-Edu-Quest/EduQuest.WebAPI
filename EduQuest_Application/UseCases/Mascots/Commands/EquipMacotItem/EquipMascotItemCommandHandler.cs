using AutoMapper;
using EduQuest_Application.DTO.Response.Mascot;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;


namespace EduQuest_Application.UseCases.Mascots.Commands.EquipMacotItem;

public class EquipMascotItemCommandHandler : IRequestHandler<EquipMascotItemCommand, APIResponse>
{
    private readonly IMascotInventoryRepository _mascotInventoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EquipMascotItemCommandHandler(
        IMascotInventoryRepository mascotInventoryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _mascotInventoryRepository = mascotInventoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(EquipMascotItemCommand request, CancellationToken cancellationToken)
    {
        var mascotItem = await _mascotInventoryRepository.GetByUserIdAndItemIdAsync(request.UserId, request.ShopItemId);
        if (mascotItem == null)
        {
            return new APIResponse
            {
                IsError = true,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.NotFound,
                    values = "item"
                }
            };
        }

        mascotItem.IsEquipped = !mascotItem.IsEquipped;
        await _mascotInventoryRepository.Update(mascotItem);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<UserMascotDto>(mascotItem);
        return new APIResponse
        {
            IsError = false,
            Payload = result,
            Message = new MessageResponse
            {
                content = MessageCommon.UpdateSuccesfully,
                values = "item"
            }
        };
    }
}