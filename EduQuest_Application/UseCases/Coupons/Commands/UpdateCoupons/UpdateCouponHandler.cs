﻿

using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Domain.Entities;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.Helper;

namespace EduQuest_Application.UseCases.Coupons.Commands.UpdateCourseCoupons;

public class UpdateCouponHandler : IRequestHandler<UpdateCouponCommand, APIResponse>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private const string Key = "name";
    private const string value = "coupon";
    public UpdateCouponHandler(ICouponRepository couponRepository, IMapper mapper, ICourseRepository courseRepository, 
        IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
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
            if(temp!.StartTime < DateTime.Now)
            {
                GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageError.CannotUpdateCoupon, Key, value);
            }
            #endregion

            temp.Discount = request.Coupon.Discount;
            temp.ExpireTime = request.Coupon.ExpireTime;
            temp.StartTime = request.Coupon.StartTime;
            temp.Description = request.Coupon.Description;
            temp.Limit = request.Coupon.Limit;
            int AllowUsage = request.Coupon.AllowUsagePerUser;
            temp.AllowUsagePerUser = AllowUsage;
            #region Validate coupon code
            if (!string.IsNullOrWhiteSpace(request.Coupon.Code))
            {
                bool couponCodeExist = await _couponRepository.ExistByCode(request.Coupon.Code);
                if (couponCodeExist)
                {
                    return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageError.CouponCodeExist, Key, value);
                }
                temp.Code = request.Coupon.Code;
            }
            ICollection<UserCoupon> userCoupons = temp.UserCoupons!;
            foreach(UserCoupon userCoupon in userCoupons)
            {
                userCoupon.RemainUsage += (AllowUsage - userCoupon.AllowUsage);
                userCoupon.AllowUsage = temp.AllowUsagePerUser;
            }
            #endregion
            temp.UpdatedAt = DateTime.Now.ToUniversalTime();
            temp.UpdatedBy = request.UserId;

            /*if (request.Coupon.WhiteListCourseIds != null)
            {
                List<Course>? existCourses = temp.Course.ToList();
                List<Course> courses = await _courseRepository.GetByListIds(request.Coupon.WhiteListCourseIds);

                HashSet<Course> existCoursesSet = new HashSet<Course>(existCourses);
                HashSet<Course> coursesSet = new HashSet<Course>(courses);

                IEnumerable<Course> courseToAdd = coursesSet.Except(existCoursesSet);
                IEnumerable<Course> courseToDelete = existCoursesSet.Except(coursesSet);

                foreach (Course course in courseToAdd)
                {
                    temp.Course.Add(course);
                }

                foreach (Course course in courseToDelete)
                {
                    temp.Course.Remove(course);
                }
            }

            if (request.Coupon.WhiteListUserIds != null)
            {
                List<User>? existUsers = temp.WhiteListUsers.ToList();
                List<User> users = await _userRepository.GetByUserIds(request.Coupon.WhiteListUserIds);

                HashSet<User> existUsersSet = new HashSet<User>(existUsers);
                HashSet<User> usersSet = new HashSet<User>(users);

                IEnumerable<User> userToAdd = usersSet.Except(existUsersSet);
                IEnumerable<User> userToDelete = existUsersSet.Except(usersSet);

                foreach (User user1 in userToAdd)
                {
                    temp.WhiteListUsers.Add(user1);
                }

                foreach (User user1 in userToDelete)
                {
                    temp.WhiteListUsers.Remove(user1);
                }
            }*/

            await _couponRepository.Update(temp);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                CouponResponse response = _mapper.Map<CouponResponse>(temp);
               /* List<string>? UserIds = temp.WhiteListUsers != null ? temp.WhiteListUsers.Select(t => t.Id).ToList() : null;
                response.WhiteListUserIds = UserIds;
                List<string>? CourseIds = temp.Course != null ? temp.Course.Select(t => t.Id).ToList() : null;
                response.WhiteListCourseIds = CourseIds;*/
                //response.CreatedByUser = _mapper.Map<CommonUserResponse>(user);
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
