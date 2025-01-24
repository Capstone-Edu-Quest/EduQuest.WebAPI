using EduQuest_Application.DTO.Request;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement
{
	public class UpdateAchievementCommand : IRequest<APIResponse>
	{
		public UpdateAchievementRequest Achievement { get; set; }

		public UpdateAchievementCommand(UpdateAchievementRequest achievement)
		{
			Achievement = achievement;
		}
	}
}
