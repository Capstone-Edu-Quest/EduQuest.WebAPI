using EduQuest_Application.Abstractions.Stripe;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Payments.Command.Refund
{
	public class RerundCommandHandler : IRequestHandler<RefundCommand, APIResponse>
	{
		private readonly StripeModel _stripeModel;
		private readonly ITransactionRepository _transactionRepository;
		private readonly ITransactionDetailRepository _transactionDetailRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IStripePayment _stripePayment;

		public RerundCommandHandler(IOptions<StripeModel> stripeModel, ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository, IUnitOfWork unitOfWork, IStripePayment stripePayment)
		{
			_stripeModel = stripeModel.Value;
			_transactionRepository = transactionRepository;
			_transactionDetailRepository = transactionDetailRepository;
			_unitOfWork = unitOfWork;
			_stripePayment = stripePayment;
		}

		public async Task<APIResponse> Handle(RefundCommand request, CancellationToken cancellationToken)
		{
			StripeConfiguration.ApiKey = _stripeModel.SecretKey;
			var transactionDetail = await _transactionDetailRepository.GetByTransactionIdAndCourseId(request.Refund.TransactionId, request.Refund.CourseId);
			var transactionExisted = await _transactionRepository.GetById(request.Refund.TransactionId);
			if(transactionDetail == null)
			{
				return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.NotFound, MessageCommon.NotFound, "name", $"Transaction Detail with ID {request.Refund.TransactionId}");
			}

			var refund = await _stripePayment.CreateRefund(transactionExisted.PaymentIntentId, (long)transactionDetail.NetAmount);

			var newTransaction = new Transaction
			{
				Id = refund.Id.ToString(),
				UserId = request.UserId,
				Status = GeneralEnums.StatusPayment.Completed.ToString(),
				TotalAmount = (decimal)transactionDetail.NetAmount,
				Type = GeneralEnums.TypeTransaction.Refund.ToString(),
			};
			await _transactionRepository.Add(newTransaction);
			await _unitOfWork.SaveChangesAsync();
			return new APIResponse
			{
				IsError = false,
				Payload = new
				{
					refundId = refund.Id,
					transactionId = transactionExisted.Id,

                }, // Trả về ID của refund
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
