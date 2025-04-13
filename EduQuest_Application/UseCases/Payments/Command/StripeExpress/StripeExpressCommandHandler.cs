using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
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
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;

		public StripeExpressCommandHandler(AccountService accountService, AccountLinkService accountLinkService, IOptions<StripeModel> stripeModel, IUserRepository userRepository, IUnitOfWork unitOfWork)
		{
			_accountService = accountService;
			_accountLinkService = accountLinkService;
			_stripeModel = stripeModel.Value;
			_userRepository = userRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(StripeExpressCommand request, CancellationToken cancellationToken)
		{
			StripeConfiguration.ApiKey = _stripeModel.SecretKey;
			var user = await _userRepository.GetById(request.UserId);
            if (user!.StripeAccountId == null)
			{
				var account = await _accountService.CreateAsync(new AccountCreateOptions
				{
					Type = "express",
					Country = "SG",
					Email = request.Email,
					Capabilities = new AccountCapabilitiesOptions
					{
						CardPayments = new AccountCapabilitiesCardPaymentsOptions { Requested = true },
						Transfers = new AccountCapabilitiesTransfersOptions { Requested = true },
					}
				});

				var accountLinkOptions = new AccountLinkCreateOptions
				{
					Account = account.Id,
					RefreshUrl = "https://eduquests.giakhang3005.com/",
					ReturnUrl = "https://eduquests.giakhang3005.com/",
					Type = "account_onboarding"
				};
				user.StripeAccountId = account.Id;
				await _userRepository.Update(user);
				await _unitOfWork.SaveChangesAsync();
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
			} else
			{
				return new APIResponse
				{
					IsError = false,
					Payload = user!.StripeAccountId,
					Errors = null,
					Message = new MessageResponse
					{
						content = MessageCommon.GetSuccesfully,
						values = new Dictionary<string, string> { { "name", "bank account" } }
					}
				};
			}
          
            
		}
	}
}
