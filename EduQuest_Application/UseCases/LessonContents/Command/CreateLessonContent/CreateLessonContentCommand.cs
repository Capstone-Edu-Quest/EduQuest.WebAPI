using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Materials.Command.CreateLessonContent
{
	public class CreateLeaningMaterialCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public List<CreateLessonContentRequest> LessonContent { get; set; }

		public CreateLeaningMaterialCommand(string userId, List<CreateLessonContentRequest> lessonContent)
		{
			UserId = userId;
			LessonContent = lessonContent;
		}
	}
}
