using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetAllUsers
{
	public class GetAllUsersQuery : IRequest<APIResponse>
	{
		public string? Status { get; set; }
    }
}
