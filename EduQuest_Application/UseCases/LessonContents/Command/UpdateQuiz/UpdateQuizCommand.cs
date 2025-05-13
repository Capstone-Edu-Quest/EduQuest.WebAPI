using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.LessonContents.Command.UpdateQuiz
{
	public class UpdateQuizCommand : IRequest<APIResponse>
	{
		public string UserId { get; set; }
		public UpdateQuizRequest Quiz { get; set; }

		public UpdateQuizCommand(string userId, UpdateQuizRequest quiz)
		{
			UserId = userId;
			Quiz = quiz;
		}
	}
}
