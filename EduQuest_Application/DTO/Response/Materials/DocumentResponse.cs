using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Materials
{
	public class DocumentResponse : IMapFrom<Material>, IMapTo<Material>
	{
        public string Id { get; set; }
        public string Type { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int? Version { get; set; }
		public string? OriginalMaterialId { get; set; }
		public double? Duration { get; set; }
		public string UserId { get; set; }
		public string? Content { get; set; }
	}
}
