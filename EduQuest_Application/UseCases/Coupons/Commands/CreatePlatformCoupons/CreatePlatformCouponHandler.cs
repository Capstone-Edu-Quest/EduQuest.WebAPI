

using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Domain.Entities;
using EduQuest_Application.DTO.Response.Coupons;
using static EduQuest_Domain.Enums.GeneralEnums;
using EduQuest_Application.Helper;

namespace EduQuest_Application.UseCases.Coupons.Commands.CreatePlatformCoupons;

public class CreatePlatformCouponHandler : IRequestHandler<CreatePlatformCouponCommand, APIResponse>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePlatformCouponHandler(ICouponRepository couponRepository, IMapper mapper, 
        IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(CreatePlatformCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region check role and authorization
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.SessionTimeout);
            }
            string role = ((int)UserRole.Admin).ToString();
            //check if user role is admin
            if (user.RoleId != role)
            {
                return CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.UserDontHavePer);
            }
            #endregion

            Coupon newCoupon = _mapper.Map<Coupon>(request.Coupon);
            newCoupon.RemainUsage = newCoupon.Usage;
            newCoupon.CreatedAt = DateTime.Now.ToUniversalTime();
            newCoupon.CreatedBy = request.UserId;
            newCoupon.IsCourseExclusive = false;
            if (request.Coupon.CustomeCode != null)
            {
                newCoupon.Code = request.Coupon.CustomeCode;
            }
            else
            {
                newCoupon.Code = CodeGenerator.GenerateRandomCouponCode();
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
        }
        catch (Exception ex)
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
