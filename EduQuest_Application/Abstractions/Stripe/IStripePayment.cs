using Stripe.Checkout;

namespace EduQuest_Application.Abstractions.Stripe;

public interface IStripePayment
{
    Task<Session> CreateStripeSessionAsync(decimal amount, string productName, string successUrl, string cancelUrl);
}
