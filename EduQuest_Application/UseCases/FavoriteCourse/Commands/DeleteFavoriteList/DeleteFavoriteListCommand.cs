using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.FavoriteCourse.Commands.DeleteFavoriteList
{
	public class DeleteFavoriteListCommand : IRequest<APIResponse>
	{
		public string UserId { get; set; }
        public string CourseId { get; set; }

		public DeleteFavoriteListCommand(string userId, string courseId)
		{
			UserId = userId;
			CourseId = courseId;
		}
	}
}
