using EduQuest_Application.Abstractions.Stripe;
using EduQuest_Infrastructure.ExternalServices.Payment.Setting;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace EduQuest_Infrastructure.ExternalServices.Payment;

public class StripePayment : IStripePayment
{
    private readonly StripeModel _stripeModel;

    public StripePayment(IOptions<StripeModel> stripeModel)
    {
        _stripeModel = stripeModel.Value;
        StripeConfiguration.ApiKey = _stripeModel.SecretKey;
    }

    public async Task<Session> CreateStripeSessionAsync(decimal amount, string currency, string productName, string successUrl, string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            Mode = "payment",
            Currency = currency,
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency,
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
}
