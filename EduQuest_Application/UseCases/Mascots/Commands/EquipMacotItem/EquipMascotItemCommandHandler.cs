using AutoMapper;
using EduQuest_Application.DTO.Response.Mascot;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static Google.Rpc.Context.AttributeContext.Types;


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
        if (request.ItemIds == null || !request.ItemIds.Any())
        {
            var userMascots = await _mascotInventoryRepository.GetMascotEquippedByUserIdAsync(request.UserId);
            var result = _mapper.Map<List<UserMascotDto>>(userMascots);
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, result, "name", "item");
        }

        await _mascotInventoryRepository.UpdateRangeMascot(request.ItemIds, request.UserId);
        await _unitOfWork.SaveChangesAsync();

        var updatedMascots = await _mascotInventoryRepository.GetMascotByUserIdAndItemIdAsync(request.UserId, request.ItemIds);
        var resultUpdate = _mapper.Map<List<UserMascotDto>>(updatedMascots);

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, resultUpdate, "name", "item");
    }

}