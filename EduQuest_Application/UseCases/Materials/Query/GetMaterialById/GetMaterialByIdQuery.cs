using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Materials.Query.GetDetailMaterial;

public class GetMaterialByIdQuery : IRequest<APIResponse>
{
    public string MaterialId { get; set; }

	public GetMaterialByIdQuery(string materialId)
	{
		MaterialId = materialId;
	}
}
