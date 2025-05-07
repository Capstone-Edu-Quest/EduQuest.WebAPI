using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Tags;

public class UserTagDto : IMapFrom<UserTag>, IMapTo<UserTag>    
{
    public string TagId { get; set; }
    public string TagName { get; set; }
}
