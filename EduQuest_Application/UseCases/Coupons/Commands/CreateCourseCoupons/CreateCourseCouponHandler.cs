

using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Domain.Entities;
using EduQuest_Application.DTO.Response.Coupons;

namespace EduQuest_Application.UseCases.Coupons.Commands.CreateCourseCoupons;

public class CreateCourseCouponHandler : IRequestHandler<CreateCourseCouponCommand, APIResponse>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseCouponHandler(ICouponRepository couponRepository, IMapper mapper, 
        ICourseRepository courseRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(CreateCourseCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region check role and authorization
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.SessionTimeout);
            }

            //check owner if it's a course exclusive coupon           
            bool isOwner = await _courseRepository.IsOwner(request.CreateCouponRequest.CourseId!, request.UserId);
            if (!isOwner)
            {
                return CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.UserDontHavePer);
            }           
            #endregion

            Coupon newCoupon = _mapper.Map<Coupon>(request.CreateCouponRequest);
            newCoupon.RemainUsage = newCoupon.Usage;
            newCoupon.CreatedAt = DateTime.Now.ToUniversalTime();
            newCoupon.CreatedBy = request.UserId;
            newCoupon.IsCourseExclusive = true;
            if (request.CreateCouponRequest.CustomeCode != null)
            {
                newCoupon.Code = request.CreateCouponRequest.CustomeCode;
            }
            else
            {
                newCoupon.Code = "test"; //random generate code function
            }
            newCoupon.Id = Guid.NewGuid().ToString();

            await _couponRepository.Add(newCoupon);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse
                {
                    IsError = false,
                    Payload = _mapper.Map<CouponResponse>(newCoupon),
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusResponse = HttpStatusCode.OK,
                        Message = MessageCommon.CreateSuccesfully
                    },
                    Message = new MessageResponse
                    {
                        content = MessageCommon.CreateSuccesfully,
                        values = new Dictionary<string, string> { { "name", "coupons" } }
                    }
                };
            }

            return CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed);
        }catch (Exception ex)
        {
            return CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    private APIResponse CreateErrorResponse(HttpStatusCode statusCode, string message)
    {
        return new APIResponse
        {
            IsError = true,
            Payload = null,
            Errors = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                StatusResponse = statusCode,
                Message = message
            },
            Message = new MessageResponse
            {
                content = MessageCommon.CreateFailed,
                values = new Dictionary<string, string> { { "name", "coupons" } }
            }
        };
    }
}
