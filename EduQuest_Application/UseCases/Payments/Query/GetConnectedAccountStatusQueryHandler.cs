using EduQuest_Application.Abstractions.Stripe;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Payments.Query
{
	public class GetConnectedAccountStatusQueryHandler : IRequestHandler<GetConnectedAccountStatusQuery, APIResponse>
	{
		private readonly IUserRepository _userRepository;
		private readonly IStripePayment _stripePayment;
		private readonly StripeModel _stripeModel;
		public GetConnectedAccountStatusQueryHandler(IUserRepository userRepository, IStripePayment stripePayment, IOptions<StripeModel> stripeModel)
		{
			_userRepository = userRepository;
			_stripePayment = stripePayment;
			_stripeModel = stripeModel.Value;
			
		}

		public async Task<APIResponse> Handle(GetConnectedAccountStatusQuery request, CancellationToken cancellationToken)
		{
			StripeConfiguration.ApiKey = _stripeModel.SecretKey;

			var user = await _userRepository.GetUserById(request.UserId);
			string status = "";
			string? stripeAccountUrl = null;

			if (user.StripeAccountId == null)
			{
				return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, status, "name", $"User not created connected account ");
			}

			status = await _stripePayment.GetStatus(user.StripeAccountId);
			if (status == StripeAccountStatus.Restricted.ToString())
			{
				stripeAccountUrl = user.StripeAccountUrl;
			}

			var responseData = new
			{
				Status = status,
				StripeAccountUrl = stripeAccountUrl
			};

			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, responseData, "name", $"Status Connected Account of User Id {request.UserId}");
		}
	}
}
