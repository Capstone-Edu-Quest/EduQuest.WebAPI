using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Payments.Command.StripeExpress
{
	public class StripeExpressCommandHandler : IRequestHandler<StripeExpressCommand, APIResponse>
	{
		private readonly Stripe.AccountService _accountService;
		private readonly AccountLinkService _accountLinkService;
		private readonly StripeModel _stripeModel;

		public StripeExpressCommandHandler(IOptions<StripeModel> stripeModel , Stripe.AccountService accountService, AccountLinkService accountLinkService)
		{
			_accountService = accountService;
			_accountLinkService = accountLinkService;
			_stripeModel = stripeModel.Value;
		}

		public async Task<APIResponse> Handle(StripeExpressCommand request, CancellationToken cancellationToken)
		{
			StripeConfiguration.ApiKey = _stripeModel.SecretKey;
			var account = await _accountService.CreateAsync(new AccountCreateOptions
			{
				Type = "express",
				Country = "SG",
				Email = request.Email,
				Capabilities = new AccountCapabilitiesOptions
				{
					CardPayments = new AccountCapabilitiesCardPaymentsOptions {  Requested = true},
					Transfers = new AccountCapabilitiesTransfersOptions { Requested = true},
				}
			});

			var accountLinkOptions = new AccountLinkCreateOptions
			{
				Account = account.Id,
				RefreshUrl = "https://eduquests.giakhang3005.com/",
				ReturnUrl = "https://eduquests.giakhang3005.com/",
				Type = "account_onboarding"
			};

			var accountLink = await _accountLinkService.CreateAsync(accountLinkOptions);
			return new APIResponse
			{
				IsError = false,
				Payload = accountLink.Url,
				Errors = null,
				Message = new MessageResponse
				{
					content = MessageCommon.CreateSuccesfully,
					values = new Dictionary<string, string> { { "name", "stripe express" } }
				}
			};
		}
	}
}
