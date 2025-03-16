using AutoMapper;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;
using System.Net;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.UseCases.Coupons.Queries.GetPlatformCouponsQuery;

public class GetPlatformCouponsHandler : IRequestHandler<GetPlatformCouponsQuery, APIResponse>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private const string Key = "name";
    private const string value = "coupon";
    public GetPlatformCouponsHandler(ICouponRepository couponRepository, IMapper mapper, 
        IUserRepository userRepository)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(GetPlatformCouponsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            #region check role and authorization
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.GetFailed, MessageCommon.SessionTimeout, Key, value);
            }
            string admin = ((int)UserRole.Admin).ToString();
            string staff = ((int)UserRole.Staff).ToString();
            /*//check if user role is admin
            if (user.RoleId != admin || user.RoleId != staff)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.GetFailed, MessageCommon.UserDontHavePer, Key, value);
            }*/
            #endregion

            var result = await _couponRepository.GetAllPlatformCoupon(request.PageNo, request.PageSize,
                request.DiscountValue, request.CouponCode, request.ExpiredAt);
            List<CouponResponse> responseDto = new List<CouponResponse>();

            var temp = result.Items.ToList();
            foreach (var item in temp)
            {
                //CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(item.User);
                CouponResponse myCouponResponse = _mapper.Map<CouponResponse>(item);
                /*myCouponResponse.WhiteListCourseIds = item.Course.Count > 0 ? item.Course.Select(c => c.Id).ToList() : null;
                myCouponResponse.WhiteListUserIds = item.WhiteListUsers.Count > 0 ? item.WhiteListUsers.Select(w => w.Id).ToList() : null;*/
                responseDto.Add(myCouponResponse);
            }
            PagedList<CouponResponse> responses = new PagedList<CouponResponse>(responseDto, result.TotalItems, result.CurrentPage, result.EachPage);
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK,MessageCommon.GetSuccesfully, responses, Key, value);
        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.GetFailed, ex.Message, Key, value);
        }
    }
}
