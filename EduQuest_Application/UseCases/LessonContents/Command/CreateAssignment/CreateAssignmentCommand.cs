using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LessonContents.Command.CreateAssignment
{
	public class CreateAssignmentCommand : IRequest<APIResponse>
	{
		public string UserId { get; set; }
		public CreateAssignmentRequest Assignment { get; set; }

		public CreateAssignmentCommand(string userId, CreateAssignmentRequest assignment)
		{
			UserId = userId;
			Assignment = assignment;
		}
	}
}
