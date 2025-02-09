using EduQuest_Application.DTO.Request.Stages;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Stages.Command.UpdateStage
{
    public class UpdateStageCommand : IRequest<APIResponse>
	{
		public List<UpdateStageRequest>? Stages { get; set; }

		public UpdateStageCommand(List<UpdateStageRequest>? stages)
		{
			Stages = stages;
		}
	}
}
