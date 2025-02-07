using EduQuest_Application.DTO.Request;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Stages.Command.CreateStage
{
	public class CreateStageCommand : IRequest<APIResponse>
	{
		public List<StageCourseRequest>? StageCourse { get; set; }

		public CreateStageCommand(List<StageCourseRequest>? stageCourse)
		{
			StageCourse = stageCourse;
		}
	}
}
