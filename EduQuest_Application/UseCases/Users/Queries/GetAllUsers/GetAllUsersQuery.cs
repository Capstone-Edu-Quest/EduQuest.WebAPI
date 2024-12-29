using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetAllUsers
{
	public class GetAllUsersQuery : IRequest<APIResponse>
	{
		public int Page { get; set; }
		public int Pagesize { get; set; }

		public GetAllUsersQuery(int page, int pagesize)
		{
			Page = page;
			Pagesize = pagesize;
		}
	}
}
