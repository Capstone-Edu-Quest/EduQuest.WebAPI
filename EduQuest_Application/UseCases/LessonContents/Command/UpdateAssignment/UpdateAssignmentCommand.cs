using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.LessonContents.Command.UpdateAssignment
{
	public class UpdateAssignmentCommand : IRequest<APIResponse>
	{
		public string UserId { get; set; }
		public AssignmentRequest Assignment { get; set; }

		public UpdateAssignmentCommand(string userId, AssignmentRequest assignment)
		{
			UserId = userId;
			Assignment = assignment;
		}
	}
}
