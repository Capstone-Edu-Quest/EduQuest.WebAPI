using AutoMapper;
using EduQuest_Application.DTO.Response.Subscriptions;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Models.Subscriptions;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Subscriptions.Query.GetSubscriptions
{
	public class GetSubscriptionsQueryHandler : IRequestHandler<GetSubscriptionsQuery, APIResponse>
	{
		private readonly ISubscriptionRepository _subscriptionRepository;
		private readonly IMapper _mapper;

		public GetSubscriptionsQueryHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
		{
			_subscriptionRepository = subscriptionRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetSubscriptionsQuery request, CancellationToken cancellationToken)
		{
			var listResult = new List<SubscriptionDtoResponse>();
			var packagePrice = new SubscriptionDtoResponse
			{
				Name = GeneralEnums.PackageName.APIsPackagePrice.ToString(),
				Data = new Dictionary<string, object>(),
			};
			var priceforInstructor = await _subscriptionRepository.GetPackgaePriceByRole((int)GeneralEnums.UserRole.Instructor);
			var priceforLearner = await _subscriptionRepository.GetPackgaePriceByRole((int)GeneralEnums.UserRole.Learner);

			packagePrice.Data.Add(GeneralEnums.UserRole.Instructor.ToString(), priceforInstructor);
			packagePrice.Data.Add(GeneralEnums.UserRole.Learner.ToString(), priceforLearner);

			listResult.Add(packagePrice);

			var packageNumber = new SubscriptionDtoResponse
			{
				Name = GeneralEnums.PackageName.APIsPackageNumbers.ToString(),
				Data = new Dictionary<string, object>(),
			};

			var packageNumberForInstructor = await _subscriptionRepository.GetPackageNumbersByRole((int)GeneralEnums.UserRole.Instructor);
			var packageNumberForLearner = await _subscriptionRepository.GetPackageNumbersByRole((int)GeneralEnums.UserRole.Learner);

			packageNumber.Data.Add(GeneralEnums.UserRole.Instructor.ToString(), packageNumberForInstructor);
			packageNumber.Data.Add(GeneralEnums.UserRole.Learner.ToString(), packageNumberForLearner);

			listResult.Add(packageNumber);
			return new APIResponse()
			{
				IsError = false,
				Payload = listResult,
				Errors = null,
				Message = new MessageResponse
				{
					content = MessageCommon.GetSuccesfully,
					values = new
					{
						name = "subscription"
					}
				}
			};


		}

	}
}
