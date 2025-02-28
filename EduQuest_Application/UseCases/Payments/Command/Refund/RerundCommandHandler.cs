using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
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
		private readonly ITransactionRepository _transactionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public RerundCommandHandler(IOptions<StripeModel> stripeModel, RefundService refundService, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
		{
			_stripeModel = stripeModel.Value;
			_refundService = refundService;
			_transactionRepository = transactionRepository;
			_unitOfWork = unitOfWork;
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
			var transactionExisted = await _transactionRepository.GetByPaymentIntentId(request.Refund.PaymentIntentId);
			


			var refund = await _refundService.CreateAsync(refundOptions);

			var newransaction = new Transaction
			{
				Id = Guid.NewGuid().ToString(),
				UserId = request.UserId,
				Status = GeneralEnums.StatusPayment.Pending.ToString(),
				TotalAmount = (decimal)request.Refund.Amount,
				Type = transactionExisted.Type,
				PaymentIntentId = refund.Id,
			};
			await _transactionRepository.Add(newransaction);
			await _unitOfWork.SaveChangesAsync();
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
