using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Tags
{
    public class TagResponse : IMapFrom<Tag>, IMapTo<Tag>
    {
        public string Name { get; set; }
        //public string? Level { get; set; }
        //public int? Grade { get; set; }
        public string? Type { get; set; }
    }
}
