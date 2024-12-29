using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response
{
	public class UserResponseTestDto : IMapFrom<User>, IMapTo<User>
	{
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string Status { get; set; } = null!;
		public string RoleId { get; set; }
	}
}
