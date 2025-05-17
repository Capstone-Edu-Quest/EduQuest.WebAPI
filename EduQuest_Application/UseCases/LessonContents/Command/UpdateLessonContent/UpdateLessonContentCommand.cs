using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LessonContents.Command.UpdateLessonContent
{
	public class UpdateLessonContentCommand : IRequest<APIResponse>
	{
		public string UserId { get; set; }
		public UpdateLessonContentRequest LessonContent { get; set; }

		public UpdateLessonContentCommand(string userId, UpdateLessonContentRequest lessonContent)
		{
			UserId = userId;
			LessonContent = lessonContent;
		}
	}
}
