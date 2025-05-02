using AutoMapper;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, APIResponse>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserTagRepository _userTagRepo;

    public UpdateUserCommandHandler(
        IMapper mapper,
        IUserRepository userRepo,
        IUserTagRepository userTagRepo,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _userRepo = userRepo;
        _userTagRepo = userTagRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepo.GetById(request.Id);
        if (existingUser == null)
        {
            return new APIResponse
            {
                IsError = true,
                Message = new MessageResponse { content = MessageCommon.NotFound, values = new { name = "user" } }
            };
        }

        if (!string.IsNullOrWhiteSpace(request.Username))
            existingUser.Username = request.Username;

        if (!string.IsNullOrWhiteSpace(request.Phone))
            existingUser.Phone = request.Phone;

        if (!string.IsNullOrWhiteSpace(request.Headline))
            existingUser.Headline = request.Headline;

        if (!string.IsNullOrWhiteSpace(request.Description))
            existingUser.Description = request.Description;

        if (request.Tags is not null && request.Tags.Any())
        {
            await _userTagRepo.DeleteByUserIdAsync(existingUser.Id);

            var newTags = request.Tags
                .Where(tagId => !string.IsNullOrEmpty(tagId))
                .Select(tagId => new UserTag
                {
                    UserId = existingUser.Id,
                    TagId = tagId!
                }).ToList();

            await _userTagRepo.BulkCreateAsync(newTags);
        }


        await _userRepo.Update(existingUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new APIResponse
        {
            IsError = false,
            Payload = _mapper.Map<UserResponseDto>(existingUser),
            Message = new MessageResponse { content = MessageCommon.UpdateSuccesfully, values = new { name = "user" } }
        };
    }
}
