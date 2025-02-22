using AutoMapper;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;
using System.Net;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.UseCases.Coupons.Queries.GetPlatformCouponsQuery;

public class GetPlatformCouponsHandler : IRequestHandler<GetPlatformCouponsQuery, APIResponse>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

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
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.GetFailed, MessageCommon.SessionTimeout, "name", "coupon");
            }
            string role = ((int)UserRole.Admin).ToString();
            //check if user role is admin
            if (user.RoleId != role)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.GetFailed, MessageCommon.UserDontHavePer, "name", "coupon");
            }
            #endregion

            var result = await _couponRepository.GetAllPlatformCoupon(request.PageNo, request.PageSize,
                request.DiscountValue, request.CouponCode, request.ExpiredAt);
            List<CouponResponse> responseDto = new List<CouponResponse>();

            var temp = result.Items.ToList();
            foreach (var item in temp)
            {
                CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(item.User);
                CouponResponse myCouponResponse = _mapper.Map<CouponResponse>(item);
                myCouponResponse.CreatedByUser = userResponse;
                responseDto.Add(myCouponResponse);
            }
            PagedList<CouponResponse> responses = new PagedList<CouponResponse>(responseDto, result.TotalItems, result.CurrentPage, result.EachPage);
            return new APIResponse
            {
                IsError = true,
                Payload = responses,
                Errors = null,
                Message = new MessageResponse
                {
                    content = MessageCommon.GetSuccesfully,
                    values = new Dictionary<string, string> { { "name", "coupon" } }
                }
            };
        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.GetFailed, ex.Message, "name", "coupon");
        }
    }
}
