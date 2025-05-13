using AutoMapper;
using EduQuest_Application.DTO.Response.Materials.DetailMaterials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LessonContents.Query.GetDetailMaterial;

public class GetMaterialByIdQueryHandler : IRequestHandler<GetMaterialByIdQuery, APIResponse>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;

    public GetMaterialByIdQueryHandler(IMaterialRepository materialRepository, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
    {
        var material = await _materialRepository.GetById(request.MaterialId);
        
        var mapper = _mapper.Map<DetailMaterialResponseDto>(material);
        //mapper.Videos.UrlMaterial = material.UrlMaterial;
        //mapper.Videos.Duration = material.Duration;
        
        return GeneralHelper.CreateSuccessResponse(
                    HttpStatusCode.OK,
                    MessageCommon.CreateSuccesfully,
                    mapper,
                    "name",
                    "Material"
                );

    }
}
