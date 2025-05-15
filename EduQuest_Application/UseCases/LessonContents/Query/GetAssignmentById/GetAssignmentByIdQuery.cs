using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LessonContents.Query.GetAssignmentById
{
	public class GetAssignmentByIdQuery : IRequest<APIResponse>
	{
        public string AssignmentId { get; set; }

		public GetAssignmentByIdQuery(string assignmentId)
		{
			AssignmentId = assignmentId;
		}
	}
}
