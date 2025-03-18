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
				Monthly = sub.MonthlyPrice.HasValue ? (decimal)sub.MonthlyPrice.Value : 0,
				Yearly = sub.YearlyPrice.HasValue ? (decimal)sub.YearlyPrice.Value : 0,
				Benefits = string.IsNullOrEmpty(sub.BenefitsJson)
					? new List<BenefitDtoResponse>()
					: JsonConvert.DeserializeObject<Dictionary<string, string>>(sub.BenefitsJson)
						.Select(b => new BenefitDtoResponse
						{
							Benefit = b.Key,
							Value = b.Value
						}).ToList() // Chuyển đổi dictionary thành danh sách BenefitDtoResponse
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
