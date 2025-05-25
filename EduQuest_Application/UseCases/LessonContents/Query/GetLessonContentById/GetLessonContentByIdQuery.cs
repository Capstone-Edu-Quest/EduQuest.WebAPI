using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LessonContents.Query.GetLessonContentById
{
	public class GetLessonContentByIdQuery : IRequest<APIResponse>
	{
        public string LessonContentId { get; set; }

		public GetLessonContentByIdQuery(string lessonContentId)
		{
			LessonContentId = lessonContentId;
		}
	}
}
