using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Payment.Command.CreateCheckout
{
	public class CreateCheckoutCommandHandler : IRequestHandler<CreateCheckoutCommand, APIResponse>
	{
		private readonly StripeModel _stripeModel;
		private readonly ITransactionRepository _transactionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CreateCheckoutCommandHandler(IOptions<StripeModel> stripeModel, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
		{
			_stripeModel = stripeModel.Value;
			_transactionRepository = transactionRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(CreateCheckoutCommand request, CancellationToken cancellationToken)
		{
			StripeConfiguration.ApiKey = _stripeModel.SecretKey;

			var options = new SessionCreateOptions
			{
				Mode = "payment",
				Currency = "usd",
				SuccessUrl = _stripeModel.SuccessUrl,
				CancelUrl = _stripeModel.CancelUrl,
				LineItems = request.Products.Select(product => new SessionLineItemOptions
				{
					Quantity = 1,
					PriceData = new SessionLineItemPriceDataOptions
					{
						Currency = "usd",
						UnitAmount = (long)(product.Price * 100),
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = product.Name,
							Description = product.Description,
						}
					}
				}).ToList()
			};

			var service = new SessionService();
			Session session = service.Create(options);

			var transaction = new Transaction
			{
				Id = session.Id,
				UserId = request.UserId,  
				TotalAmount = request.Products.Sum(p => p.Price), 
				Status = StatusPayment.Pending.ToString(), 
				PaymentIntentId = session.PaymentIntentId,
				
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
