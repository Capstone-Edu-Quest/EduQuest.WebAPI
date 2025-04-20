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
	public class RefundCommandHandler : IRequestHandler<RefundCommand, APIResponse>
	{
		private readonly StripeModel _stripeModel;
		private readonly ITransactionRepository _transactionRepository;
		private readonly ITransactionDetailRepository _transactionDetailRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILearnerRepository _learnerRepository;
		private readonly IUserRepository _userRepository;
		private readonly IStripePayment _stripePayment;

		public RefundCommandHandler(IOptions<StripeModel> stripeModel, 
			ITransactionRepository transactionRepository, 
			ITransactionDetailRepository transactionDetailRepository, 
			IUnitOfWork unitOfWork, ILearnerRepository learnerRepository, IUserRepository userRepository, IStripePayment stripePayment)
		{
			_stripeModel = stripeModel.Value;
			_transactionRepository = transactionRepository;
			_transactionDetailRepository = transactionDetailRepository;
			_unitOfWork = unitOfWork;
			_learnerRepository = learnerRepository;
			_userRepository = userRepository;
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
			var user = await _userRepository.GetUserById(transactionExisted.UserId);
			var newTransaction = new Transaction
			{
				Id = refund.Id.ToString(),
				UserId = request.UserId,
				Status = GeneralEnums.StatusPayment.Completed.ToString(),
				TotalAmount = (decimal)transactionDetail.NetAmount,
				Type = GeneralEnums.TypeTransaction.Refund.ToString(),
				CustomerEmail = user.Email,
				CustomerName = user.Username
			};
			await _transactionRepository.Add(newTransaction);
			var learner = await _learnerRepository.GetByUserIdAndCourseId(transactionExisted.UserId, request.Refund.CourseId);
			if (learner == null)
			{
				return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.NotFound, MessageCommon.NotFound, "name", $"User in Course {request.Refund.CourseId}");
			}
			learner.IsActive = false;
			await _learnerRepository.Update(learner);
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
