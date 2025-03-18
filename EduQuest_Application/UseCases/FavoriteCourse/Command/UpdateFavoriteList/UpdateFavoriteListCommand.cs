using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.FavoriteCourse.Command.AddFavoriteList
{
	public class UpdateFavoriteListCommand : IRequest<APIResponse>
	{
		public string UserId { get; set; }
		public List<string> CourseId { get; set; }

		public UpdateFavoriteListCommand(string userId, List<string> courseId)
		{
			UserId = userId;
			CourseId = courseId;
		}
	}
}
