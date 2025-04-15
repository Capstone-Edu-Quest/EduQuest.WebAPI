using Stripe;
using Stripe.Checkout;

namespace EduQuest_Application.Abstractions.Stripe;

public interface IStripePayment
{
    Task<Session> CreateStripeSessionAsync(decimal amount, string productName, string successUrl, string cancelUrl);
	Task<string> GetStatus(string accountId);
	Task<Refund> CreateRefund(string paymentIntentId, long amount);
}
