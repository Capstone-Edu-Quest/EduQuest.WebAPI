

using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Domain.Entities;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Application.Helper;
using EduQuest_Application.DTO.Response.LearningPaths;

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
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.CreateFailed, MessageCommon.SessionTimeout, "name", "coupon");
            }

            //check owner if it's a course exclusive coupon           
            bool isOwner = await _courseRepository.IsOwner(request.CreateCouponRequest.CourseId!, request.UserId);
            if (!isOwner)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.CreateFailed, MessageCommon.UserDontHavePer, "name", "coupon");
            }           
            #endregion

            Coupon newCoupon = _mapper.Map<Coupon>(request.CreateCouponRequest);
            newCoupon.RemainUsage = newCoupon.Usage;
            newCoupon.CreatedAt = DateTime.Now.ToUniversalTime();
            newCoupon.CreatedBy = request.UserId;
            newCoupon.IsCourseExclusive = true;
            #region Validate coupon code
            if (!string.IsNullOrWhiteSpace(request.CreateCouponRequest.CustomeCode))
            {
                bool temp = await _couponRepository.ExistByCode(request.CreateCouponRequest.CustomeCode);
                if (temp)
                {
                    return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageError.CouponCodeExist, "name", "coupon");
                }
                newCoupon.Code = request.CreateCouponRequest.CustomeCode;
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

        }catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, ex.Message, "name", "coupon");
        }
    }
}
