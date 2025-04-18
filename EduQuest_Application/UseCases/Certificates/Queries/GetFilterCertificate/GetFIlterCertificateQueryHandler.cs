using AutoMapper;
using EduQuest_Application.DTO.Response.Certificates;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Pagination;
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
        var query = await _certificateRepository.GetCertificatesWithFilters(request.Id, request.UserId, request.CourseId);

        var certificates = _mapper.Map<List<CertificateDto>>(query);

        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, certificates, "name", "certificate");
    }
}
