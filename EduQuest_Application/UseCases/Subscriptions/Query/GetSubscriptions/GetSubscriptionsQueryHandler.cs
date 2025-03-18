using AutoMapper;
using EduQuest_Application.DTO.Response.Subscriptions;
using EduQuest_Domain.Entities;
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
			var subscriptions = await _subscriptionRepository.GetAll();
			var subscriptionDtos = subscriptions.Select(sub => new SubscriptionDtoResponse
			{
				Package = sub.Package,
				Monthly = (decimal)sub.MonthlyPrice!,
				Yearly = (decimal)sub.YearlyPrice!,
				Benefits = string.IsNullOrEmpty(sub.BenefitsJson)
				? new List<BenefitDtoResponse>()
				: JsonConvert.DeserializeObject<List<BenefitDtoResponse>>(sub.BenefitsJson)  // Giải mã JSON
			}).ToList();

			return new APIResponse
			{
				IsError = false,
				Payload = subscriptionDtos,
				Errors = null,
				Message = new MessageResponse
				{
					content = MessageCommon.GetSuccesfully,
					values = new Dictionary<string, string> { { "name", "subcription"} }
				}
			};
		}
	}
}
