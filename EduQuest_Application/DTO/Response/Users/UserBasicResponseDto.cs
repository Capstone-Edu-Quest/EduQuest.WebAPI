using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Users
{
	public class UserBasicResponseDto : IMapFrom<User>, IMapTo<User>
	{
		public string? Id { get; set; }
		public string? Username { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string Status { get; set; } = null!;
		public string Headline { get; set; }
		public string Description { get; set; }
		public string AvatarUrl { get; set; }
		public string RoleId { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
