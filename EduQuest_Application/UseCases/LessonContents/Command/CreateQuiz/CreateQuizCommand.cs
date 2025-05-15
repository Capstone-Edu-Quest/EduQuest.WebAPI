using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LessonContents.Command.CreateQuiz
{
	public class CreateQuizCommand : IRequest<APIResponse>
	{
		public string UserId { get; set; }
		public CreateQuizRequest Quiz { get; set; }

		public CreateQuizCommand(string userId, CreateQuizRequest quiz)
		{
			UserId = userId;
			Quiz = quiz;
		}
	}
}
