using AutoMapper;
using EduQuest_Application.DTO.Response.Subscriptions;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Subscription.Query.GetSubscriptions
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
				Data = new Dictionary<int, RolePackageDto>(),
			};
			var priceforInstructor = await _subscriptionRepository.GetPackgaePriceByRole((int)GeneralEnums.UserRole.Instructor);
			var priceforLearner = await _subscriptionRepository.GetPackgaePriceByRole((int)GeneralEnums.UserRole.Learner);

			packagePrice.Data.Add((int)GeneralEnums.UserRole.Instructor, priceforInstructor);
			packagePrice.Data.Add((int)GeneralEnums.UserRole.Learner, priceforLearner);

			listResult.Add(packagePrice);



		}

	}
}
