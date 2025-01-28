using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request
{
	public class UpdateQuestRequest : IMapFrom<Quest>, IMapTo<Quest>
	{
        public string Id { get; set; }
        public string Name { get; set; }
		public string Description { get; set; }
		public List<string> ListBadgeId { get; set; }
	}
}
