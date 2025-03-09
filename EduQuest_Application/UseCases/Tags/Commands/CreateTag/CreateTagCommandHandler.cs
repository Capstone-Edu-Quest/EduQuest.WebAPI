using AutoMapper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Tags.Commands.CreateTag;

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;

    public CreateTagCommandHandler(IUnitOfWork unitOfWork, ITagRepository tagRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var existTag = await _tagRepository.GetTagByName(request.TagName!);
        if (existTag == null)
        {
            var tagEntity = new Tag
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.TagName,
            };

            await _tagRepository.Add(tagEntity);
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                IsError = false,
                Payload = null,
                Message = new MessageResponse
                {
                    content = MessageCommon.CreateSuccesfully,
                    values = new
                    {
                        name = "tag"
                    }
                }
            };
        }

        return new APIResponse
        {
            IsError = true,
            Payload = null,
            Message = new MessageResponse
            {
                content = MessageCommon.CreateFailed,
                values = new
                {
                    name = "tag"
                }
            }
        };

    }
}
