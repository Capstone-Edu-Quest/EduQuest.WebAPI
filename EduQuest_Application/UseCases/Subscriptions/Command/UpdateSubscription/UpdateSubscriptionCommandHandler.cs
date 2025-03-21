using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;

namespace EduQuest_Application.UseCases.Subscriptions.Command.UpdateSubscription
{
	public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, APIResponse>
	{
		private readonly ISubscriptionRepository _subscriptionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
		{
			_subscriptionRepository = subscriptionRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
		{
			var listUpdate = new List<Subscription>();
			foreach(var subscription in request.Subscriptions)
			{
				var sub = await _subscriptionRepository.GetSubscriptionByRoleIPackageConfig(subscription.RoleId, subscription.PackageEnum, subscription.ConfigEnum);
				if(sub != null)
				{
					sub.Value = subscription.Value;
					listUpdate.Add(sub);
				}
			}
			await _subscriptionRepository.UpdateRangeAsync(listUpdate);
			var result = await _unitOfWork.SaveChangesAsync() > 0;
			return new APIResponse
			{
				IsError = !result,
				Payload = result ? listUpdate : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.UpdateFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.UpdateSuccesfully : MessageCommon.UpdateFailed,
					values = new Dictionary<string, string> { { "name", "subscription" } }
				}
			};

		}
	}
}
