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
        if (!request.ItemIds.Any() || request.UserId == null)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.OK, MessageCommon.NotFound,
            MessageCommon.NotFound, "name", "item");
        }

        await _mascotInventoryRepository.UpdateRangeMascot(request.ItemIds, request.UserId);
        await _unitOfWork.SaveChangesAsync();
        var userMascots = await _mascotInventoryRepository.GetMascotByUserIdAndItemIdAsync(request.UserId, request.ItemIds);
        var result = _mapper.Map<List<UserMascotDto>>(userMascots);
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.UpdateSuccesfully,
            result, "name", "item");
    }
}