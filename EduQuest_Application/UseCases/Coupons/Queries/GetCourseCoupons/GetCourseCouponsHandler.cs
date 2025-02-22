using AutoMapper;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Coupons.Queries.GetCourseCoupons;

public class GetCourseCouponsHandler : IRequestHandler<GetCourseCouponsQuery, APIResponse>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private const string Key = "name";
    private const string value = "coupon";
    public GetCourseCouponsHandler(ICouponRepository couponRepository, IMapper mapper, 
        ICourseRepository courseRepository, IUserRepository userRepository)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
        _courseRepository = courseRepository;
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(GetCourseCouponsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            #region check role and authorization
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.GetFailed, MessageCommon.SessionTimeout, Key, value);
            }
            //check if user is a owner if the course        
            bool isOwner = await _courseRepository.IsOwner(request.CourseId, request.UserId);
            if (!isOwner)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.GetFailed, MessageCommon.UserDontHavePer, Key, value);
            }
            #endregion

            var result = await _couponRepository.GetAllByCourseId(request.CourseId, request.PageNo, request.PageSize,
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
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully, responses, Key, value);
        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.GetFailed, ex.Message, Key, value);
        }
    }
}
