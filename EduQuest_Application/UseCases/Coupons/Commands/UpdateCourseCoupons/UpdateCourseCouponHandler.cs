

using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Domain.Entities;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.Helper;

namespace EduQuest_Application.UseCases.Coupons.Commands.UpdateCourseCoupons;

public class UpdateCourseCouponHandler : IRequestHandler<UpdateCourseCouponCommand, APIResponse>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private const string Key = "name";
    private const string value = "coupon";
    public UpdateCourseCouponHandler(ICouponRepository couponRepository, IMapper mapper, ICourseRepository courseRepository, 
        IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(UpdateCourseCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region check role and authorization
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.UpdateFailed, MessageCommon.SessionTimeout, Key, value);
            }
            //check coupon is exist
            Coupon? temp = await _couponRepository.GetById(request.CouponId);
            if(temp == null)
            {
                GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.UpdateFailed, MessageCommon.NotFound, Key, value);
            }
            //check owner if it's a course exclusive coupon           
            bool isOwner = await _courseRepository.IsOwner(temp.CourseId!, request.UserId);
            if (!isOwner)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.UpdateFailed, MessageCommon.UserDontHavePer, Key, value);
            }
            #endregion

            temp.DiscountValue = request.Coupon.DiscountValue;
            temp.ExpireAt = request.Coupon.ExpireAt;
            if(request.Coupon.AddedUsage != 0)
            {
                temp.Usage += request.Coupon.AddedUsage;
                temp.Usage = Math.Max(temp.Usage, -1); // Đảm bảo không nhỏ hơn -1
                if(temp.Usage > 0)
                {
                    temp.RemainUsage += request.Coupon.AddedUsage;
                    temp.RemainUsage = Math.Max(temp.RemainUsage, 0);// Đảm bảo không nhỏ hơn 0
                }
                temp.RemainUsage += request.Coupon.AddedUsage;
                temp.RemainUsage = Math.Max(temp.RemainUsage, -1);// Đảm bảo không nhỏ hơn -1 
            }
            temp.UpdatedAt = DateTime.Now.ToUniversalTime();
            temp.UpdatedBy = request.UserId;
            

            await _couponRepository.Update(temp);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                CouponResponse response = _mapper.Map<CouponResponse>(temp);
                response.CreatedByUser = _mapper.Map<CommonUserResponse>(user);
                return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, response, Key, value);
            }

            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.UpdateFailed, Key, value);

        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, ex.Message, Key, value);
        }
    }
}
