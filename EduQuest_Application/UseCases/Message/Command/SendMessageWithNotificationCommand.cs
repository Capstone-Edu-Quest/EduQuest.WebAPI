using EduQuest_Application.DTO.Request;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases
{
	public class SendMessageWithNotificationCommand : IRequest<APIResponse>
	{
        public SendMessageRequest Request { get; set; }

		public SendMessageWithNotificationCommand(SendMessageRequest request)
		{
			Request = request;
		}
	}
}
