

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

public class CreateCouponHandler : IRequestHandler<CreateCouponCommand, APIResponse>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private const string Key = "name";
    private const string value = "coupon";
    public CreateCouponHandler(ICouponRepository couponRepository, IMapper mapper, 
        ICourseRepository courseRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region check role and authorization
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.CreateFailed, MessageCommon.SessionTimeout, Key, value);
            }         
            #endregion

            Coupon newCoupon = _mapper.Map<Coupon>(request.CreateCouponRequest);
            newCoupon.Usage = 0;
            newCoupon.CreatedAt = DateTime.Now.ToUniversalTime();
            newCoupon.CreatedBy = request.UserId;
            #region Validate coupon code
            if (!string.IsNullOrWhiteSpace(request.CreateCouponRequest.Code))
            {
                bool temp = await _couponRepository.ExistByCode(request.CreateCouponRequest.Code);
                if (temp)
                {
                    return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageError.CouponCodeExist, Key, value);
                }
                newCoupon.Code = request.CreateCouponRequest.Code;
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

            if (request.CreateCouponRequest.WhiteListCourseIds != null)
            {
                newCoupon.Course = await _courseRepository.GetByListIds(request.CreateCouponRequest.WhiteListCourseIds);
            }


            List<UserCoupon> userCoupons = new List<UserCoupon>();
            if(request.CreateCouponRequest.WhiteListUserIds != null)
            {
                foreach (var item in request.CreateCouponRequest.WhiteListUserIds!)
                {
                    UserCoupon userCoupon = new UserCoupon
                    {
                        UserId = item,
                        CouponId = newCoupon.Id,
                        AllowUsage = newCoupon.AllowUsagePerUser,
                        RemainUsage = newCoupon.AllowUsagePerUser,
                    };
                    userCoupons.Add(userCoupon);
                }
            }
                
            newCoupon.WhiteListUsers = userCoupons;

            await _couponRepository.Add(newCoupon);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                CouponResponse response = _mapper.Map<CouponResponse>(newCoupon);
                List<string>? UserIds = newCoupon.WhiteListUsers.Select(t => t.UserId).ToList();
                response.WhiteListUserIds = UserIds;
                List<string>? CourseIds = newCoupon.Course.Select(t => t.Id).ToList();
                response.WhiteListCourseIds = CourseIds;
                //response.CreatedByUser = _mapper.Map<CommonUserResponse>(user);
                return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.CreateSuccesfully, response, Key, value);
            }
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageCommon.CreateFailed, Key, value);

        }catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, ex.Message, Key, value);
        }
    }
}
