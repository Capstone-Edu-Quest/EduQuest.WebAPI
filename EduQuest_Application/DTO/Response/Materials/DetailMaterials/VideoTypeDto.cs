using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Materials.DetailMaterials;

public class VideoTypeDto : IMapFrom<Material>, IMapTo<Material>
{
    public string? UrlMaterial { get; set; }
    public double? Duration { get; set; }
    public string? Thumbnail { get; set; }
}
