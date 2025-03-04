using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Payments.Command.CreateCheckout
{
	public class CreateCheckoutCommandHandler : IRequestHandler<CreateCheckoutCommand, APIResponse>
	{
		private readonly StripeModel _stripeModel;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICartRepository _cartRepository;
		private readonly ITransactionRepository _transactionRepository;

		public CreateCheckoutCommandHandler(IOptions<StripeModel> stripeModel, IUnitOfWork unitOfWork, ICartRepository cartRepository, ITransactionRepository transactionRepository)
		{
			_stripeModel = stripeModel.Value;
			_unitOfWork = unitOfWork;
			_cartRepository = cartRepository;
			_transactionRepository = transactionRepository;
		}

		public async Task<APIResponse> Handle(CreateCheckoutCommand request, CancellationToken cancellationToken)
		{
			StripeConfiguration.ApiKey = _stripeModel.SecretKey;
			var cart = await _cartRepository.GetById(request.CartId);

			var options = new SessionCreateOptions
			{
				Mode = "payment",
				Currency = "usd",
				SuccessUrl = _stripeModel.SuccessUrl,
				CancelUrl = _stripeModel.CancelUrl,
				LineItems = new List<SessionLineItemOptions>
				{
					new SessionLineItemOptions
					{
						Quantity = 1,
						PriceData = new SessionLineItemPriceDataOptions
						{
							Currency = "usd",
							UnitAmount = (long)(cart.Total * 100), // Đảm bảo nhân 100 vì Stripe dùng cents
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = "Course" 
							}
						}
					}
				}
			};

			var service = new SessionService();
			Session session = service.Create(options);

			var transaction = new Transaction
			{
				Id = Guid.NewGuid().ToString(),
				UserId = request.UserId,
				Status = GeneralEnums.StatusPayment.Pending.ToString(),
				TotalAmount = (decimal)cart.Total,
				Type = (request.CartId != null)? GeneralEnums.TypeTransaction.CheckoutCart.ToString() : GeneralEnums.TypeTransaction.Account.ToString(),
				PaymentIntentId = session.Id,
			};
			await _transactionRepository.Add(transaction);

			await _unitOfWork.SaveChangesAsync();

			return new APIResponse
			{
				IsError = false,
				Payload = session.Url,
				Errors = null,
				Message = new MessageResponse
				{
					content = MessageCommon.CreateSuccesfully,
					values = new Dictionary<string, string> { { "name", "payment"} }
				}
			};
		}
	}
}
