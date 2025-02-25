using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.CartItems.Command
{
	public class AddCartItemCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public List<string> CourseIds { get; set; }

		public AddCartItemCommand(string userId, List<string> courseIds)
		{
			UserId = userId;
			CourseIds = courseIds;
		}
	}
}
