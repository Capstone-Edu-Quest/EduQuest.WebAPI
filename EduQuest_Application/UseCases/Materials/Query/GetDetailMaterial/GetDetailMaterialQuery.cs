using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Materials.Query.GetDetailMaterial;

public class GetDetailMaterialQuery : IRequest<APIResponse>
{
    public string? userId { get; set; }

    public GetDetailMaterialQuery(string? userId)
    {
        this.userId = userId;
    }
}
