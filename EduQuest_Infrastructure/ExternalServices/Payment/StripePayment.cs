using EduQuest_Application.Abstractions.Stripe;
using EduQuest_Domain.Entities;
using EduQuest_Infrastructure.ExternalServices.Payment.Setting;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Stripe.TestHelpers;

namespace EduQuest_Infrastructure.ExternalServices.Payment;

public class StripePayment : IStripePayment
{
    private readonly StripeModel _stripeModel;
	private readonly Stripe.RefundService _refundService;

	public StripePayment(IOptions<StripeModel> stripeModel, Stripe.RefundService refundService)
    {
        _stripeModel = stripeModel.Value;
		_refundService= refundService;
		StripeConfiguration.ApiKey = _stripeModel.SecretKey;
    }

	public async Task<Refund> CreateRefund(string paymentIntentId, long amount)
	{
		var refundOptions = new RefundCreateOptions
		{
			PaymentIntent = paymentIntentId,
			Amount = (long)amount * 100,
			Reason = "requested_by_customer"
		};

		var refund = await _refundService.CreateAsync(refundOptions);
        return refund;
	}

	public async Task<Session> CreateStripeSessionAsync(decimal amount, string productName, string successUrl, string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            Mode = "payment",
            Currency = "usd",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(amount * 100), // cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = productName
                        }
                    }
                }
            }
		};

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return session;
    }

	public async Task<string> GetStatus(string accountId)
	{
		var accountService = new AccountService();
		var account = await accountService.GetAsync(accountId);

		// Có thể dùng account.DetailsSubmitted, account.Capabilities, account.Requirements để kiểm tra thêm
		return account.Requirements?.DisabledReason == null ? "Complete" : "Restricted";
	}
}
