using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Application.ExternalServices.QuartzService;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Notification;
using EduQuest_Domain.Models.Payment;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Transactions.Command.UpdateTransactionStatus
{
    public class UpdateTransactionStatusCommandHandler : IRequestHandler<UpdateTransactionStatusCommand, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseStatisticRepository _courseStatisticRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IFireBaseRealtimeService _firebaseRealtimeService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuartzService _quartzService;
        private readonly StripeModel _stripeModel;

        public UpdateTransactionStatusCommandHandler(ITransactionRepository transactionRepository, 
            ICourseRepository courseRepository, 
            ICourseStatisticRepository courseStatisticRepository, 
            ILessonRepository lessonRepository, 
            ITransactionDetailRepository transactionDetailRepository, 
            IUserRepository userRepository, 
            ICartRepository cartRepository, 
            ISubscriptionRepository subscriptionRepository, 
            IFireBaseRealtimeService firebaseRealtimeService, 
            IUnitOfWork unitOfWork, 
            IQuartzService quartzService, 
           IOptions<StripeModel> stripeModel)
        {
            _transactionRepository = transactionRepository;
            _courseRepository = courseRepository;
            _courseStatisticRepository = courseStatisticRepository;
            _lessonRepository = lessonRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _subscriptionRepository = subscriptionRepository;
            _firebaseRealtimeService = firebaseRealtimeService;
            _unitOfWork = unitOfWork;
            _quartzService = quartzService;
            _stripeModel = stripeModel.Value;
        }

        public async Task<APIResponse> Handle(UpdateTransactionStatusCommand request, CancellationToken cancellationToken)
        {
            StripeConfiguration.ApiKey = _stripeModel.SecretKey;
            var transactionExisted = await _transactionRepository.GetById(request.TransactionId);
            var user = await _userRepository.GetUserById(transactionExisted.UserId);
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
            //var chargeService = new ChargeService();
            //var chargeList = await chargeService.ListAsync(new ChargeListOptions
            //{
            //	PaymentIntent = transactionExisted.PaymentIntentId, // Lọc theo PaymentIntent
            //	Limit = 1
            //});

            var (balanceTransaction, errorResponse) = await GetBalanceTransactionFromPaymentIntent(request.PaymentIntentId);
            if (errorResponse != null)
            {
                return errorResponse;
            }


            //Update Transaction
            decimal netAmount = balanceTransaction.Net / 100m;
            transactionExisted.NetAmount = netAmount;
            transactionExisted.StripeFee = transactionExisted.TotalAmount - netAmount;
            transactionExisted.Status = request.Status;
            transactionExisted.PaymentIntentId = request.PaymentIntentId;
            transactionExisted.CustomerName = request.CustomerName;
            transactionExisted.CustomerEmail = request.CustomerEmail;
            transactionExisted.Url = null;
            await _transactionRepository.Update(transactionExisted);

            //Update Trasaction Detail

            var transactionDetailList = await _transactionDetailRepository.GetByTransactionId(request.TransactionId);
            if (transactionDetailList.Any() && (transactionDetailList.FirstOrDefault(x => x.TransactionId == request.TransactionId).ItemType == GeneralEnums.ItemTypeTransactionDetail.Course.ToString()))
            {
                var myCart = await _cartRepository.GetByUserId(transactionExisted.UserId);
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

                List<string> purchasedCourseNames = new();
                foreach (var detail in transactionDetailList)
                {
                    var cartItem = myCart.CartItems.FirstOrDefault(c => c.CourseId == detail.ItemId);
                    decimal? systemShare, instructorShare;

                    if (cartItem != null)
                    {
                        var course = await _courseRepository.GetById(cartItem.CourseId);
                        purchasedCourseNames.Add(course.Title);
                        if (course == null)
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
                                    values = new Dictionary<string, string> { { "name", $"course with Id {cartItem.CourseId}" } }
                                }
                            };
                        }
                        var instructor = await _userRepository.GetById(course.CreatedBy);

                        //Calculate Stripe fee
                        decimal? percentage = (cartItem.Price / myCart.Total) * 100;
                        decimal? stripeFeeForInstructor = (percentage / 100) * transactionExisted.StripeFee;

                        //Calculate amount after fees
                        decimal courseNetAmount = cartItem.Price - (decimal)stripeFeeForInstructor;
                        int packageEnum = (int)Enum.Parse(typeof(PackageEnum), user.Package);
                        var courseFeeForPlatForm = await _subscriptionRepository.GetSubscriptionByRoleIPackageConfig(((int)GeneralEnums.UserRole.Instructor).ToString(), packageEnum, (int)GeneralEnums.ConfigEnum.CommissionFee);
                        if (detail.ItemType == GeneralEnums.ItemTypeTransactionDetail.Course.ToString())
                        {
                            systemShare = Math.Round(courseNetAmount * ((decimal)(courseFeeForPlatForm.Value) / 100), 2);
                            instructorShare = courseNetAmount - systemShare;
                            //Update for transaction detail
                            detail.StripeFee = Math.Round((decimal)stripeFeeForInstructor, 2);
                            detail.NetAmount = courseNetAmount;
                            detail.SystemShare = systemShare;
                            detail.InstructorShare = Math.Round((decimal)instructorShare, 2);
                        }
                        var firstLessonAndMaterialId = await _lessonRepository.GetFirstLessonAndMaterialIdInCourseAsync(course.Id);
                        //if (firstLessonAndMaterialId.lessonId == null && firstLessonAndMaterialId.materialId == null)
                        //{
                        //    return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, $"Not Found Any Lesson", "name", $"Lesson in Course ID {course.Id}");
                        //}

                        var newLearner = new CourseLearner
                        {
                            CourseId = course.Id,
                            UserId = transactionExisted.UserId,
                            IsActive = true,
                            TotalTime = 0,
                            ProgressPercentage = 0,
                            CurrentLessonId = firstLessonAndMaterialId.lessonId != null ? firstLessonAndMaterialId.lessonId : null,
                            CurrentMaterialId = firstLessonAndMaterialId.materialId != null ?  firstLessonAndMaterialId.materialId : null,
                            CreatedAt = DateTime.Now.ToUniversalTime(),
                            UpdatedAt = DateTime.Now.ToUniversalTime(),

                        };
                        course.CourseLearners.Add(newLearner);
                        course.CourseStatistic.TotalLearner++;
                        await _courseStatisticRepository.Update(course.CourseStatistic);
                        await _courseRepository.Update(course);
                    }
                }
                myCart.CartItems.Clear();
                await _cartRepository.Delete(myCart.Id);
                await _transactionDetailRepository.UpdateRangeAsync(transactionDetailList);
				await _quartzService.TransferToInstructor(transactionExisted.Id);


                await _firebaseRealtimeService.PushNotificationAsync(
                                new NotificationDto
                                {
                                    userId = transactionExisted.UserId,
                                    Content = NotificationMessage.BUY_COURSE_SUCCESSFULLY,
                                    Receiver = transactionExisted.UserId,
                                    Url = "",
                                    Values = new Dictionary<string, string>
                                    {
                                        { "item", string.Join(",", purchasedCourseNames)} 
                                    }
                                }
                            );
            }
            else
            {
                foreach (var detail in transactionDetailList)
                {
                    detail.StripeFee = transactionExisted.TotalAmount - netAmount;
                    detail.NetAmount = netAmount;
                    detail.SystemShare = netAmount;
                    var subscription = await _subscriptionRepository.GetById(detail.ItemId);
                    user.Package = PackageEnum.Pro.ToString();
                    if (detail.ItemType.ToLower() == ConfigEnum.PriceYearly.ToString().ToLower())
                    {
                        user.PackageExperiedDate = DateTime.Now.ToUniversalTime().AddMonths(30);
						await _quartzService.UpdateUserPackageAccountMonthly(user.Id);
					}
                    else if (detail.ItemType.ToLower() == ConfigEnum.PriceMonthly.ToString().ToLower())
                    {
                        user.PackageExperiedDate = DateTime.Now.ToUniversalTime().AddYears(1);
						await _quartzService.UpdateUserPackageAccountYearLy(user.Id);
					}
                    await _userRepository.Update(user);
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

        private async Task<(BalanceTransaction balanceTransaction, APIResponse error)> GetBalanceTransactionFromPaymentIntent(string paymentIntentId)
        {
            var chargeService = new ChargeService();
            var balanceTransactionService = new BalanceTransactionService();
            Charge charge = null;

            try
            {
                for (int attempt = 1; attempt <= 3; attempt++)
                {
                    var chargeList = await chargeService.ListAsync(new ChargeListOptions
                    {
                        PaymentIntent = paymentIntentId,
                        Limit = 1
                    });

                    charge = chargeList.Data.FirstOrDefault();

                    if (charge != null &&
                        charge.Status?.ToLower() == "succeeded" &&
                        !string.IsNullOrEmpty(charge.BalanceTransactionId))
                    {
                        try
                        {
                            var balanceTransaction = await balanceTransactionService.GetAsync(charge.BalanceTransactionId);
                            return (balanceTransaction, null);
                        }
                        catch
                        {
                            //tiếp tục retry
                        }
                    }

                    await Task.Delay(2000);
                }
            }
            catch (Exception ex)
            {
                // Nuốt lỗi tổng thể để không lọt ra Global Exception Middleware
            }

            var error = new APIResponse
            {
                IsError = true,
                Errors = new ErrorResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Transaction not successful or balance transaction not ready",
                },
                Message = new MessageResponse
                {
                    content = "Transaction not successful or balance transaction not ready",
                    values = new Dictionary<string, string> { { "name", "transaction" } }
                }
            };

            return (null, error);
        }


    }
}
