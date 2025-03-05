using AutoMapper;
using EduQuest_Application.DTO.Response.Tags;
using EduQuest_Application.UseCases.Tag.Queries.GetFilterTag;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Certificates.Queries.GetFilterCertificate;

public class GetFilterTagQueryHandler : IRequestHandler<GetFilterTagQuery, APIResponse>
{
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;

    public GetFilterTagQueryHandler(ITagRepository tagRepository, IMapper mapper)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetFilterTagQuery request, CancellationToken cancellationToken)
    {
        var query = await _tagRepository.GetTagsWithFilters(request.TagId, request.Name, request.Page, request.EachPage);

        var certificates = _mapper.Map<PagedList<TagDto>>(query);

        return new APIResponse
        {
            IsError = false,
            Payload = certificates,
            Message = new MessageResponse
            {
                content = MessageCommon.GetSuccesfully,
                values = new
                {
                    name = "tag"
                }
            }
        };
    }
}
