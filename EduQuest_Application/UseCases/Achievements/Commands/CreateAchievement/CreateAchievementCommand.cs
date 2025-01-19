using EduQuest_Application.DTO.Request;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Achievements.Commands.CreateAchievement
{
	public class CreateAchievementCommand : IRequest<APIResponse>
	{
        public CreateAchievementRequest Achievement { get; set; }

		public CreateAchievementCommand(CreateAchievementRequest achievement)
		{
			Achievement = achievement;
		}
	}
}
