using AutoMapper;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.DTO.Response.Materials.DetailMaterials;

namespace EduQuest_Application.UseCases.Materials.Query.GetDetailMaterial;

public class GetDetailMaterialQueryHandler : IRequestHandler<GetDetailMaterialQuery, APIResponse>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;

    public GetDetailMaterialQueryHandler(IMaterialRepository materialRepository, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetDetailMaterialQuery request, CancellationToken cancellationToken)
    {
        var result = await _materialRepository.GetByUserIdAsync(request.userId);
        var mapper = _mapper.Map<List<DetailMaterialResponseDto>>(result);

        return GeneralHelper.CreateSuccessResponse(
                    HttpStatusCode.OK,
                    MessageCommon.CreateSuccesfully,
                    mapper,
                    "name",
                    "Material"
                );

    }
}
