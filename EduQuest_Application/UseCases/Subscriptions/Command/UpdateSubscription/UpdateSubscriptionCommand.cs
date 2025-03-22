using EduQuest_Application.DTO.Request.Subscriptions;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Subscriptions.Command.UpdateSubscription
{
	public class UpdateSubscriptionCommand : IRequest<APIResponse>
	{
        public List<SubscriptionRequest> Subscriptions { get; set; }

		public UpdateSubscriptionCommand(List<SubscriptionRequest> subscriptions)
		{
			Subscriptions = subscriptions;
		}
	}
}
