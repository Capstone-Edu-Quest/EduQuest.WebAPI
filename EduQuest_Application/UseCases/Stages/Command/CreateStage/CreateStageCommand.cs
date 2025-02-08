using EduQuest_Application.DTO.Request.Stages;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Stages.Command.CreateStage
{
    public class CreateStageCommand : IRequest<APIResponse>
	{
        public string CourseId { get; set; }
        public List<CreateStageRequest>? StageCourse { get; set; }

		public CreateStageCommand(string courseId, List<CreateStageRequest>? stageCourse)
		{
			CourseId = courseId;
			StageCourse = stageCourse;
		}
	}
}
