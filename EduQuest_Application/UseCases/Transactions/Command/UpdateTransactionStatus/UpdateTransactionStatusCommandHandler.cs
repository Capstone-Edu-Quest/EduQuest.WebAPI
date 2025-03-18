using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Transactions.Command.UpdateTransactionStatus
{
	public class UpdateTransactionStatusCommandHandler : IRequestHandler<UpdateTransactionStatusCommand, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
        private readonly IUnitOfWork _unitOfWork;
		private readonly StripeModel _stripeModel;

		public UpdateTransactionStatusCommandHandler(ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository, ICartRepository cartRepository, ISystemConfigRepository systemConfigRepository, IUnitOfWork unitOfWork, IOptions<StripeModel> stripeModel)
		{
			_transactionRepository = transactionRepository;
			_transactionDetailRepository = transactionDetailRepository;
			_cartRepository = cartRepository;
			_systemConfigRepository = systemConfigRepository;
			_unitOfWork = unitOfWork;
			_stripeModel = stripeModel.Value;
		}

		public async Task<APIResponse> Handle(UpdateTransactionStatusCommand request, CancellationToken cancellationToken)
        {
			StripeConfiguration.ApiKey = _stripeModel.SecretKey;
			var transactionExisted = await _transactionRepository.GetByPaymentIntentId(request.TransactionId);
            if (transactionExisted == null)
            {
                return new APIResponse
                {
                    IsError = true,
                    Payload = null,
                    Errors = new ErrorResponse
                    {
                        StatusResponse = HttpStatusCode.NotFound,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = MessageCommon.NotFound,
                    },
                    Message = new MessageResponse
                    {
                        content = MessageCommon.NotFound,
                        values = new Dictionary<string, string> { { "name", $"transaction ID {request.TransactionId}" } }
                    }
                };
            }
			var chargeService = new ChargeService();
			var chargeList = await chargeService.ListAsync(new ChargeListOptions
			{
				PaymentIntent = transactionExisted.PaymentIntentId, // Lọc theo PaymentIntent
				Limit = 1
			});

			var charge = chargeList.Data.FirstOrDefault();
			if (charge == null || charge.Status != "succeeded")
			{
				return new APIResponse
				{
					IsError = true,
					Payload = null,
					Errors = new ErrorResponse
					{
						StatusResponse = HttpStatusCode.BadRequest,
						StatusCode = (int)HttpStatusCode.BadRequest,
						Message = "Transaction not successful",
					},
					Message = new MessageResponse
					{
						content = "Transaction not successful",
						values = new Dictionary<string, string> { { "name", "transaction" } }
					}
				};
			}

			var balanceTransactionService = new BalanceTransactionService();
            var balanceTransaction = await balanceTransactionService.GetAsync(charge.BalanceTransactionId);
            
            //Update Transaction
            decimal netAmount = balanceTransaction.Net / 100m;
            transactionExisted.NetAmount = netAmount;
            transactionExisted.StripeFee = transactionExisted.TotalAmount - netAmount;
			transactionExisted.Status = request.Status;
            await _transactionRepository.Update(transactionExisted);

            //Update Trasaction Detail
           
			var transactionDetailList = await _transactionDetailRepository.GetByTransactionId(request.TransactionId);
            if (transactionDetailList.Any() && (transactionDetailList.FirstOrDefault(x => x.TransactionId == request.TransactionId).ItemType == GeneralEnums.ItemTypeTransaction.Course.ToString()))
            {
				var myCart = await _cartRepository.GetByUserId(request.UserId);
				if (myCart == null)
				{
					return new APIResponse
					{
						IsError = true,
						Payload = null,
						Errors = new ErrorResponse
						{
							StatusResponse = HttpStatusCode.NotFound,
							StatusCode = (int)HttpStatusCode.NotFound,
							Message = MessageCommon.NotFound,
						},
						Message = new MessageResponse
						{
							content = MessageCommon.NotFound,
							values = new Dictionary<string, string> { { "name", "cart" } }
						}
					};
				}
				foreach (var detail in transactionDetailList)
                {
					var cartItem = myCart.CartItems.FirstOrDefault(c => c.CourseId == detail.ItemId);
					decimal? systemShare, instructorShare;
					if (cartItem != null)
                    {
                        //Calculate Stripe fee
                        decimal? percentage = (cartItem.Price/myCart.Total) * 100;
						decimal? stripeFeeForInstructor = (percentage / 100) * transactionExisted.StripeFee;

                        //Calculate amount after fees
						decimal? courseNetAmount = cartItem.Price - stripeFeeForInstructor;
                        

						var courseFeeForPlatForm = await _systemConfigRepository.GetByName(GeneralEnums.Fee.CourseFee.ToString());
                        if(detail.ItemType == GeneralEnums.ItemTypeTransaction.Course.ToString())
                        {
                            systemShare = courseNetAmount * (decimal)(courseFeeForPlatForm.Value);
                            instructorShare = courseNetAmount - systemShare;

                            //Update for transaction detail
							detail.StripeFee = stripeFeeForInstructor;
							detail.NetAmount = courseNetAmount;
							detail.SystemShare = systemShare;
							detail.InstructorShare = instructorShare;
						} 
					}
				}
            } else if (transactionDetailList.Any() && (transactionDetailList.FirstOrDefault(x => x.TransactionId == request.TransactionId).ItemType == GeneralEnums.ItemTypeTransaction.ProAccount.ToString()))
			{
				foreach (var detail in transactionDetailList)
				{
					detail.StripeFee = transactionExisted.TotalAmount - netAmount;
					detail.NetAmount = netAmount;
					detail.SystemShare = netAmount;
				}
			}
            var result = await _unitOfWork.SaveChangesAsync() > 0;


            return new APIResponse
            {
                IsError = !result,
                Payload = result ? transactionExisted : null,
                Errors = result ? null : new ErrorResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = MessageCommon.SavingFailed,
                },
                Message = new MessageResponse
                {
                    content = result ? MessageCommon.CreateSuccesfully : MessageCommon.CreateFailed,
                    values = new Dictionary<string, string> { { "name", "transaction" } }
                }
            };
        }
    }
}
