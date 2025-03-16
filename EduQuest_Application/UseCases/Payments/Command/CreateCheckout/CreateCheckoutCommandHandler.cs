using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Stripe;
using Stripe.Checkout;

using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Payments.Command.CreateCheckout
{
	public class CreateCheckoutCommandHandler : IRequestHandler<CreateCheckoutCommand, APIResponse>
	{
		private readonly StripeModel _stripeModel;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICartRepository _cartRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly ITransactionDetailRepository _transactionDetailRepository;
		private readonly ICourseRepository _courseRepository;



		public async Task<APIResponse> Handle(CreateCheckoutCommand request, CancellationToken cancellationToken)
		{
			StripeConfiguration.ApiKey = _stripeModel.SecretKey;
			var cart = await _cartRepository.GetListCartItemByCartId(request.CartId);

			var options = new SessionCreateOptions
			{
				Mode = "payment",
				Currency = "usd",
				SuccessUrl = _stripeModel.SuccessUrl,
				CancelUrl = _stripeModel.CancelUrl,
				LineItems = new List<SessionLineItemOptions>
				{
					new SessionLineItemOptions
					{
						Quantity = 1,
						PriceData = new SessionLineItemPriceDataOptions
						{
							Currency = "usd",
							UnitAmount = (long)(cart.Total * 100), // Đảm bảo nhân 100 vì Stripe dùng cents
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = "Course"
							}
						}
					}
				}
			};
			var service = new SessionService();
			Session session = service.Create(options);
			var paymentIntentId = session.Id;

			//Transaction
			var transaction = new Transaction();
			if (cart.Total > 0)
			{
				transaction.Id = Guid.NewGuid().ToString();
				transaction.UserId = request.UserId;
				transaction.Status = GeneralEnums.StatusPayment.Pending.ToString();
				transaction.TotalAmount = (decimal)cart.Total;
				transaction.Type = (request.CartId != null) ? GeneralEnums.TypeTransaction.CheckoutCart.ToString() : GeneralEnums.TypeTransaction.Account.ToString();
				transaction.PaymentIntentId = paymentIntentId;
				await _transactionRepository.Add(transaction);
			}

			//Transaction Detail
			var cartItems = cart.CartItems;
			List<TransactionDetail> transactionDetails = new List<TransactionDetail>();
			if (cartItems.Any())
			{
				foreach (var item in cartItems)
				{
					var course = await _courseRepository.GetById(item.CourseId);
					var transactionDetail = new TransactionDetail
					{
						Id = Guid.NewGuid().ToString(),
						TransactionId = transaction.Id,
						InstructorId = course.CreatedBy,
						ItemId = item.CourseId,
						ItemType = GeneralEnums.ItemTypeTransaction.Course.ToString(),
						Amount = item.Price,
					};

					transactionDetails.Add(transactionDetail);
				}
				await _transactionDetailRepository.CreateRangeAsync(transactionDetails);
				//await _cartRepository.Delete(cart.Id);
				await _unitOfWork.SaveChangesAsync();
				return new APIResponse
				{
					IsError = false,
					Payload = session.Url,
					Errors = null,
					Message = new MessageResponse
					{
						content = MessageCommon.CreateSuccesfully,
						values = new Dictionary<string, string> { { "name", "payment" } }
					}
				};
			}
			return new APIResponse
			{
				IsError = true,
				Payload = null,
				Errors = null,
				Message = new MessageResponse
				{
					content = MessageCommon.NotFound,
					values = new Dictionary<string, string> { { "name", "cartitem" } }
				}
			};
		}
	}
}
