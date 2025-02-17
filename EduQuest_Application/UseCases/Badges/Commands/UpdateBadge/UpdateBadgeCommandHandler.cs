using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.UseCases.Badges.Commands.UpdateBadge;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository.Generic;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using EduQuest_Application.DTO.Response.Badges;

namespace EduQuest_Application.UseCases.Badges.Commands.UpdateBadge;

public class UpdateBadgeCommandHandler : IRequestHandler<UpdateBadgeCommand, APIResponse>
{
    private readonly IGenericRepository<Badge> _badgeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBadgeCommandHandler(IGenericRepository<Badge> badgeRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _badgeRepository = badgeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(UpdateBadgeCommand request, CancellationToken cancellationToken)
    {
        // Tìm Badge theo ID
        var existingBadge = await _badgeRepository.GetById(request.Id);
        if (existingBadge == null)
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
                    values = "badge"
                }
            };
        }

        existingBadge.Name = request.Name ?? existingBadge.Name;
        existingBadge.Description = request.Description ?? existingBadge.Description;
        existingBadge.IconUrl = request.IconUrl ?? existingBadge.IconUrl;
        existingBadge.Color = request.Color ?? existingBadge.Color;
        existingBadge.UpdatedAt = DateTime.UtcNow;

        await _badgeRepository.Update(existingBadge);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<BadgeDto>(existingBadge);

        return new APIResponse
        {
            IsError = false,
            Payload = result,
            Message = new MessageResponse
            {
                content = MessageCommon.UpdateSuccesfully,
                values = "badge"
            }
        };
    }
}
