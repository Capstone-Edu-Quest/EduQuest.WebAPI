using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response.Certificates;

public class CertificateDto : IMapFrom<Certificate>, IMapTo<Certificate>
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string UserId { get; set; }
    public string CourseId { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<PagedList<Certificate>, PagedList<CertificateDto>>().ReverseMap();
    }
}
