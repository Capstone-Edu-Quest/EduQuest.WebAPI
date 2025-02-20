using AutoMapper;
using EduQuest_Application.DTO.Response.Certificate;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Certificates.Queries.GetFilterCertificate;

public class GetCertificatesQueryHandler : IRequestHandler<GetCertificatesQuery, APIResponse>
{
    private readonly ICertificateRepository _certificateRepository;
    private readonly IMapper _mapper;

    public GetCertificatesQueryHandler(ICertificateRepository certificateRepository, IMapper mapper)
    {
        _certificateRepository = certificateRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetCertificatesQuery request, CancellationToken cancellationToken)
    {
        var query = _certificateRepository.GetCertificatesWithFilters(request.Title, request.UserId, request.CourseId);

        var certificates = _mapper.Map<IEnumerable<CertificateDto>>(query.ToList());

        return new APIResponse
        {
            IsError = false,
            Payload = certificates,
            Message = new MessageResponse
            {
                content = MessageCommon.GetSuccesfully,
                values = "certificates"
            }
        };
    }
}
