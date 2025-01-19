using EduQuest_Application.UseCases.FavoriteCourse.Commands.AddFavoriteList;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Achievements.Commands.CreateAchievement
{
	public class CreateAchievementCommandHandler : IRequestHandler<CreateAchievementCommand, APIResponse>
	{
		public Task<APIResponse> Handle(CreateAchievementCommand request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
