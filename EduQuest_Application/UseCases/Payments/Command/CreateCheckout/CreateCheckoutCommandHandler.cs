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
		private readonly IPaymentRepository _paymentRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CreateCheckoutCommandHandler(IOptions<StripeModel> stripeModel, IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
		{
			_stripeModel = stripeModel.Value;
			_paymentRepository = paymentRepository;
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

			//var payment = new Payment
			//{

			//}

			
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
