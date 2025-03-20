using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Subscriptions.Command.UpdateSubscription
{
	public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, APIResponse>
	{
		public Task<APIResponse> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
