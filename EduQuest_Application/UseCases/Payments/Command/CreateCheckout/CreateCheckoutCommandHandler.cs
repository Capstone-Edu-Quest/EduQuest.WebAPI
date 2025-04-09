using EduQuest_Application.Helper;
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
		private readonly ILearnerRepository _learnerRepository;

		public CreateCheckoutCommandHandler(IOptions<StripeModel> stripeModel, IUnitOfWork unitOfWork, ICartRepository cartRepository, ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository, ICourseRepository courseRepository, ISubscriptionRepository subscriptionRepository, IUserRepository userRepository, ICouponRepository couponRepository, ILearnerRepository learnerRepository)
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
			_learnerRepository = learnerRepository;
		}

		public async Task<APIResponse> Handle(CreateCheckoutCommand request, CancellationToken cancellationToken)
		{
			StripeConfiguration.ApiKey = _stripeModel.SecretKey;
			Cart? cart = new Cart();
			var subscription = new EduQuest_Domain.Entities.Subscription();
			var options = new SessionCreateOptions
			{
				Mode = "payment",
				Currency = "usd",
				SuccessUrl = _stripeModel.SuccessUrl,
				CancelUrl = _stripeModel.CancelUrl,
				LineItems = new List<SessionLineItemOptions>()
			};
			var user = await _userRepository.GetById(request.UserId);
			
			if (request.Request.CartId != null)
			{
				cart = await _cartRepository.GetListCartItemByCartId(request.Request.CartId);
				if (cart == null)
				{
					return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", "cart");
				}
				if(cart.Total > 0)
				{
					if (request.Request.CouponCode != null) //Incase cart has coupon
					{
						var IsCouponAvai = await _couponRepository.IsCouponAvailable(request.Request.CouponCode, request.UserId);
						if (IsCouponAvai)
						{
							var isConsumeCoupon = await _couponRepository.ConsumeCoupon(request.Request.CouponCode, request.UserId);
							if (isConsumeCoupon)
							{
								var coupon = await _couponRepository.GetCouponByCode(request.Request.CouponCode);
								var disCount = coupon.Discount * cart.Total;
								if(cart.Total - disCount == 0)
								{
									var listCourseId = cart.CartItems.Select(x => x.CourseId).ToList();
									var response = await CreateCourseLearners(listCourseId, request.UserId);
									cart.CartItems.Clear();
									await _cartRepository.Delete(cart.Id);
									var result = await _unitOfWork.SaveChangesAsync();
									return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageLearner.AddedUserToCourse, response, "name", "learner");
								} else
								{
									var listCoursePriceZero = new List<string>();
									foreach (var item in cart.CartItems)
									{
										var course = await _courseRepository.GetById(item.CourseId);
										var finalPrice = course.Price - (coupon.Discount * course.Price); 
										if (finalPrice == 0)
										{
											listCoursePriceZero.Add(course.Id);
										}
										cart.CartItems.Remove(item);
										
									}
									var listLearnerCourseFree = await CreateCourseLearners(listCoursePriceZero, request.UserId);

									var result = await _unitOfWork.SaveChangesAsync();
									options.LineItems.Add(new SessionLineItemOptions
									{
										Quantity = 1,
										PriceData = new SessionLineItemPriceDataOptions
										{
											Currency = "usd",
											UnitAmount = (long)((cart.Total - disCount) * 100), // Convert to cents
											ProductData = new SessionLineItemPriceDataProductDataOptions
											{
												Name = GeneralEnums.ItemTypeTransactionDetail.Course.ToString()
											}
										}
									});
								}

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
								Name = GeneralEnums.ItemTypeTransactionDetail.Course.ToString()
							}
						}
					});
				} else //Cart toal is == 0
				{
					var listCourseId = cart.CartItems.Select(x => x.CourseId).ToList();
					var response = await CreateCourseLearners(listCourseId, request.UserId); //Add all cours ein cart into table Learner
					cart.CartItems.Clear();
					await _cartRepository.Delete(cart.Id);
					var result = await _unitOfWork.SaveChangesAsync();
					return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageLearner.AddedUserToCourse, response, "name", "learner");
				}
				
			}
			else
			{
				subscription = await _subscriptionRepository.GetSubscriptionByRoleIPackageConfig(user!.RoleId!, (int)GeneralEnums.PackageEnum.Pro, (int)request.Request.ConfigEnum!);
				if(subscription == null)
				{
					return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", "subscription");
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
			var paymentIntentId = session.PaymentIntentId;

			//Create Transaction
			var transaction = new Transaction();
			transaction.Id = session.Id;
			transaction.UserId = request.UserId;
			transaction.Status = GeneralEnums.StatusPayment.Pending.ToString();
			transaction.PaymentIntentId = paymentIntentId;
			transaction.Type = (request.Request.CartId != null) ? GeneralEnums.TypeTransaction.CheckoutCart.ToString() : subscription.Config;
			transaction.Url = session.Url;

			//Transaction Detail
			List<TransactionDetail> transactionDetails = new List<TransactionDetail>();

			if (!string.IsNullOrEmpty(request.Request.CartId))
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
								ItemType = GeneralEnums.ItemTypeTransactionDetail.Course.ToString(),
								Amount = item.Price
							});
						}
					} else
					{
						return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", "cartitem");
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
				user.Package = subscription.PackageType;
				await _userRepository.Update(user);
			}
			await _transactionRepository.Add(transaction);
            await _unitOfWork.SaveChangesAsync();
            await _transactionDetailRepository.CreateRangeAsync(transactionDetails);
			//await _cartRepository.Delete(cart.Id);
			await _unitOfWork.SaveChangesAsync();
			if(request.Request.SuccessUrl != null && request.Request.CancelUrl != null)
			{
				_stripeModel.SuccessUrl = request.Request.SuccessUrl;
				_stripeModel.CancelUrl = request.Request.CancelUrl;
			}
			
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.CreateSuccesfully, session.Url, "name", "payment");
		}

		private async Task<List<CourseLearner>> CreateCourseLearners(List<string> courseIds, string userId)
		{
			
			var listCourseLearner = courseIds.Distinct().Select(courseId => new CourseLearner
			{
				Id = Guid.NewGuid().ToString(),
				CourseId = courseId,
				UserId = userId,
				IsActive = true,
				TotalTime = 0,
				ProgressPercentage = 0
			}).ToList();

			await _learnerRepository.CreateRangeAsync(listCourseLearner);
				
			return listCourseLearner;	
		}
	}
}
