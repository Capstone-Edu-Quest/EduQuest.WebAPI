using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response
{
	public class TagResponse : IMapFrom<Tag>, IMapTo<Tag>
	{
        public string Name { get; set; }
    }
}
