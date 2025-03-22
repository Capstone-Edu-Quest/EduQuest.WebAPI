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
		private readonly IUserRepository _userRepository;
		private readonly ICouponRepository _couponRepository;

		public CreateCheckoutCommandHandler(IOptions<StripeModel> stripeModel, 
			IUnitOfWork unitOfWork, 
			ICartRepository cartRepository, 
			ITransactionRepository transactionRepository, 
			ITransactionDetailRepository transactionDetailRepository, 
			ICourseRepository courseRepository, 
			ISubscriptionRepository subscriptionRepository, 
			IUserRepository userRepository, 
			ICouponRepository couponRepository)
		{
			_stripeModel = stripeModel.Value;
			_unitOfWork = unitOfWork;
			_cartRepository = cartRepository;
			_transactionRepository = transactionRepository;
			_transactionDetailRepository = transactionDetailRepository;
			_courseRepository = courseRepository;
			_subscriptionRepository = subscriptionRepository;
			_userRepository = userRepository;
			_couponRepository = couponRepository;
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
			var user = await _userRepository.GetById(request.UserId);
			if (request.PackageEnum != (int)GeneralEnums.PackageEnum.Free)
			{
				request.PackageEnum = (int)GeneralEnums.PackageEnum.Pro;
			}
			var subscription = await _subscriptionRepository.GetSubscriptionByRoleIPackageConfig(user!.RoleId!, (int)request.PackageEnum, (int)request.ConfigEnum!);
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
				if (request.CouponCode != null)
				{
					var IsCouponAvai = await _couponRepository.IsCouponAvailable(request.CouponCode, request.UserId);
					if (IsCouponAvai)
					{
						var isConsumeCoupon = await _couponRepository.ConsumeCoupon(request.CouponCode, request.UserId);
						if (isConsumeCoupon)
						{
							var coupon = await _couponRepository.GetCouponByCode(request.CouponCode);
							var disCount = coupon.Discount * cart.Total;
							options.LineItems.Add(new SessionLineItemOptions
							{
								Quantity = 1,
								PriceData = new SessionLineItemPriceDataOptions
								{
									Currency = "usd",
									UnitAmount = (long)((cart.Total - disCount) * 100), // Convert to cents
									ProductData = new SessionLineItemPriceDataProductDataOptions
									{
										Name = subscription.Config
									}
								}
							});
						}
					}
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
							Name = subscription.Config
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

				options.LineItems.Add(new SessionLineItemOptions
				{
					Quantity = 1,
					PriceData = new SessionLineItemPriceDataOptions
					{
						Currency = "usd",
						UnitAmount = (long)subscription!.Value * 100, // Convert to cents for Stripe
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = subscription.Config
						}
					}
				});
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
			transaction.Type = (request.CartId != null) ? GeneralEnums.TypeTransaction.CheckoutCart.ToString() : subscription.Config;

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
				transaction.TotalAmount = subscription!.Value; 
				transactionDetails.Add(new TransactionDetail
				{
					Id = Guid.NewGuid().ToString(),
					TransactionId = transaction.Id,
					ItemId = subscription.Id,
					ItemType = subscription.Config,
					Amount = subscription!.Value 
				});
				user.Subscriptions.Add(subscription);
				await _userRepository.Update(user);
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
