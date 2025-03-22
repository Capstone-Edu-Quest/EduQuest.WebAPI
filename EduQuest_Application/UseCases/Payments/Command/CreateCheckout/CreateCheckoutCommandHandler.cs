using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Net;
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
		private readonly ISubscriptionRepository _subscriptionRepository;

		public CreateCheckoutCommandHandler(IOptions<StripeModel> stripeModel, IUnitOfWork unitOfWork, ICartRepository cartRepository, ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository, ICourseRepository courseRepository, ISubscriptionRepository subscriptionRepository)
		{
			_stripeModel = stripeModel.Value;
			_unitOfWork = unitOfWork;
			_cartRepository = cartRepository;
			_transactionRepository = transactionRepository;
			_transactionDetailRepository = transactionDetailRepository;
			_courseRepository = courseRepository;
			_subscriptionRepository = subscriptionRepository;
		}

		public async Task<APIResponse> Handle(CreateCheckoutCommand request, CancellationToken cancellationToken)
		{
			StripeConfiguration.ApiKey = _stripeModel.SecretKey;
			Cart? cart = null;
			var options = new SessionCreateOptions
			{
				Mode = "payment",
				Currency = "usd",
				SuccessUrl = _stripeModel.SuccessUrl,
				CancelUrl = _stripeModel.CancelUrl,
				LineItems = new List<SessionLineItemOptions>()
			};
			var subscription = await _subscriptionRepository.GetById(request.ProAccountType!);
			if (request.CartId != null)
			{
				cart = await _cartRepository.GetListCartItemByCartId(request.CartId);
				if (cart == null)
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

				options.LineItems.Add(new SessionLineItemOptions
				{
					Quantity = 1,
					PriceData = new SessionLineItemPriceDataOptions
					{
						Currency = "usd",
						UnitAmount = (long)(cart.Total * 100), // Convert to cents
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							//Name = GeneralEnums.Fee.CourseFee.ToString()
						}
					}
				});
			}
			else
			{
				
				if(subscription == null)
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
							values = new Dictionary<string, string> { { "name", "subscription" } }
						}
					};
				}
				
				//options.LineItems.Add(new SessionLineItemOptions
				//{
				//	Quantity = 1,
				//	PriceData = new SessionLineItemPriceDataOptions
				//	{
				//		Currency = "usd",
				//		UnitAmount = (long)subscription!.Price * 100, // Convert to cents for Stripe
				//		ProductData = new SessionLineItemPriceDataProductDataOptions
				//		{
				//			Name = subscription.Name
				//		}
				//	}
				//});
			}

			var service = new SessionService();
			Session session = service.Create(options);
			var paymentIntentId = session.Id;

			//Create Transaction
			var transaction = new Transaction();
			transaction.Id = Guid.NewGuid().ToString();
			transaction.UserId = request.UserId;
			transaction.Status = GeneralEnums.StatusPayment.Pending.ToString();
			transaction.PaymentIntentId = paymentIntentId;
			transaction.Type = (request.CartId != null) ? GeneralEnums.TypeTransaction.CheckoutCart.ToString() : GeneralEnums.TypeTransaction.ProAccount.ToString();

			//Transaction Detail
			List<TransactionDetail> transactionDetails = new List<TransactionDetail>();

			if (request.CartId != null)
			{
				var cartItems = cart.CartItems;
				if (cart.Total > 0)
				{
					transaction.TotalAmount = (decimal)cart.Total;
					if (cartItems.Any())
					{
						foreach (var item in cartItems)
						{
							var course = await _courseRepository.GetById(item.CourseId);
							transactionDetails.Add(new TransactionDetail
							{
								Id = Guid.NewGuid().ToString(),
								TransactionId = transaction.Id,
								InstructorId = course.CreatedBy,
								ItemId = item.CourseId,
								ItemType = GeneralEnums.ItemTypeTransaction.Course.ToString(),
								Amount = item.Price
							});
						}
					} else
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
								values = new Dictionary<string, string> { { "name", "cartitem" } }
							}
						};
					}
				}
			} else
			{
				//transaction.TotalAmount = (decimal)subscription!.MonthlyPrice; //Check Month or year
				transactionDetails.Add(new TransactionDetail
				{
					Id = Guid.NewGuid().ToString(),
					TransactionId = transaction.Id,
					ItemId = subscription.Id,
					ItemType = GeneralEnums.ItemTypeTransaction.ProAccount.ToString(),
					//Amount = (decimal)subscription!.MonthlyPrice //Check Month or year
				});
			}
			await _transactionRepository.Add(transaction);
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
	}
}
