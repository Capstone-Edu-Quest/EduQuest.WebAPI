using EduQuest_Application.DTO.Request.Lessons;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Stages.Command.CreateStage
{
	public class CreateStageCommand : IRequest<APIResponse>
	{
        public string CourseId { get; set; }
        public List<CreateLessonRequest>? StageCourse { get; set; }

		public CreateStageCommand(string courseId, List<CreateLessonRequest>? stageCourse)
		{
			CourseId = courseId;
			StageCourse = stageCourse;
		}
	}
}
