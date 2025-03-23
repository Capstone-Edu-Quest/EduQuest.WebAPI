using EduQuest_Application.DTO.Request.Lessons;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Stages.Command.UpdateStage
{
	public class UpdateStageCommand : IRequest<APIResponse>
	{
		public List<UpdateLessonRequest>? Stages { get; set; }

		public UpdateStageCommand(List<UpdateLessonRequest>? stages)
		{
			Stages = stages;
		}
	}
}
