using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Payments.Command.Refund
{
	public class RerundCommandHandler : IRequestHandler<RefundCommand, APIResponse>
	{
		private readonly StripeModel _stripeModel;
		private readonly RefundService _refundService;

		public RerundCommandHandler(IOptions<StripeModel> stripeModel, RefundService refundService)
		{
			_stripeModel = stripeModel.Value;
			_refundService = refundService;
		}

		public async Task<APIResponse> Handle(RefundCommand request, CancellationToken cancellationToken)
		{
			StripeConfiguration.ApiKey = _stripeModel.SecretKey;

			var refundOptions = new RefundCreateOptions
			{
				PaymentIntent = request.Refund.PaymentIntentId,
				Amount = (long)request.Refund.Amount * 100,
				Reason = "requested_by_customer"
			};

			
			var refund = await _refundService.CreateAsync(refundOptions);
			return new APIResponse
			{
				IsError = false,
				Payload = refund.Id, // Trả về ID của refund
				Errors = null,
				Message = new MessageResponse
				{
					content = "Refund processed successfully!",
					values = new Dictionary<string, string> { { "refund_id", refund.Id } }
				}
			};
		}
	}
}
