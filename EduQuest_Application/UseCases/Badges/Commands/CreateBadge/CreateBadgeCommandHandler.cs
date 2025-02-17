using AutoMapper;
using EduQuest_Application.DTO.Response.Badges;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.Generic;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Badges.Commands.CreateBadge
{
    public class CreateBadgeHandler : IRequestHandler<CreateBadgeCommand, APIResponse>
    {
        private readonly IGenericRepository<Badge> _badgeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateBadgeHandler(IGenericRepository<Badge> badgeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _badgeRepository = badgeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(CreateBadgeCommand request, CancellationToken cancellationToken)
        {

            var badge = new Badge
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                IconUrl = request.IconUrl,
                Color = request.Color,
                CreatedAt = DateTime.UtcNow
            };

            await _badgeRepository.Add(badge);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<BadgeDto>(badge);

            return new APIResponse
            {
                IsError = false,
                Payload = result,
                Message = new MessageResponse
                {
                    content = MessageCommon.CreateSuccesfully,
                    values = "badge"
                }
            };
        }
    }
}
