using EduQuest_Application.DTO.Request.UserMetas;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.UserMetas.Commands.UpdateUserProgress
{
	public class UpdateUserProgressCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public UpdateUserProgressRequest Info { get; set; }

		public UpdateUserProgressCommand(string userId, UpdateUserProgressRequest info)
		{
			UserId = userId;
			Info = info;
		}
	}
}
