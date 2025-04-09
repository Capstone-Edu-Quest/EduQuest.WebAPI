using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Users.Queries.GetUserByRole
{
	public class GetUserByRoleQuery : IRequest<APIResponse>
	{
        public string RoleId { get; set; }
		public GetUserByRoleQuery(string roleId)
		{
			RoleId = roleId;
		}
	}
}
