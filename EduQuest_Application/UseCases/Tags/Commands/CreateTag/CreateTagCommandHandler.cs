using AutoMapper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

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
        var tagType = Enum.GetName(typeof(TagType), request.Type);
        if (existTag == null)
        {
            var tagEntity = new Tag
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.TagName,
                Level = request.Level,
                Grade = request.Grade,
                Type = tagType
            };

            await _tagRepository.Add(tagEntity);
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                IsError = false,
                Payload = tagEntity,
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
                content = MessageCommon.AlreadyExists,
                values = new
                {
                    name = "tag"
                }
            }
        };

    }
}
