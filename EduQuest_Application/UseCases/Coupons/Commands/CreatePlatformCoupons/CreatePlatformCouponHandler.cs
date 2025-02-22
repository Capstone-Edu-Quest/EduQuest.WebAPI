

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
using EduQuest_Application.DTO.Response.LearningPaths;

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
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.CreateFailed, MessageCommon.SessionTimeout, "name", "coupon");
            }
            string role = ((int)UserRole.Admin).ToString();
            //check if user role is admin
            if (user.RoleId != role)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.CreateFailed, MessageCommon.UserDontHavePer, "name", "coupon");
            }
            #endregion

            Coupon newCoupon = _mapper.Map<Coupon>(request.Coupon);
            newCoupon.RemainUsage = newCoupon.Usage;
            newCoupon.CreatedAt = DateTime.Now.ToUniversalTime();
            newCoupon.CreatedBy = request.UserId;
            newCoupon.IsCourseExclusive = false;
            #region Validate coupon code
            if (!string.IsNullOrWhiteSpace(request.Coupon.CustomeCode))
            {
                bool temp = await _couponRepository.ExistByCode(request.Coupon.CustomeCode);
                if (temp)
                {
                    return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageError.CouponCodeExist, "name", "coupon");
                }
                newCoupon.Code = request.Coupon.CustomeCode;
            }
            else
            {
                string code;
                do
                {
                    code = CodeGenerator.GenerateRandomCouponCode();
                } while (await _couponRepository.ExistByCode(code));

                newCoupon.Code = code;
            }
            #endregion
            newCoupon.Id = Guid.NewGuid().ToString();

            await _couponRepository.Add(newCoupon);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                CouponResponse response = _mapper.Map<CouponResponse>(newCoupon);
                response.CreatedByUser = _mapper.Map<CommonUserResponse>(user);
                return new APIResponse
                {
                    IsError = false,
                    Payload = response,
                    Errors = null,
                    Message = new MessageResponse
                    {
                        content = MessageCommon.CreateSuccesfully,
                        values = new Dictionary<string, string> { { "name", "coupons" } }
                    }
                };
            }

            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageCommon.CreateFailed, "name", "coupon");

        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, ex.Message, "name", "coupon");
        }
    }
}
