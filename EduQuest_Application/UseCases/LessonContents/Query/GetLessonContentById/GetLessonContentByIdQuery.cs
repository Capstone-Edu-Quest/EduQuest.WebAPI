using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LessonContents.Query.GetLessonContentById
{
	public class GetLessonContentByIdQuery : IRequest<APIResponse>
	{
        public int Type { get; set; }
        public string LessonContentId { get; set; }

		public GetLessonContentByIdQuery(int type, string lessonContentId)
		{
			Type = type;
			LessonContentId = lessonContentId;
		}
	}
}
