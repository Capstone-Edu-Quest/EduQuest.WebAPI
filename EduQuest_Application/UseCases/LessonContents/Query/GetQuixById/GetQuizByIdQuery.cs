using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LessonContents.Query.GetQuixById
{
	public class GetQuizByIdQuery : IRequest<APIResponse>
	{
        public string QuizId { get; set; }

		public GetQuizByIdQuery(string quizId)
		{
			QuizId = quizId;
		}
	}
}
