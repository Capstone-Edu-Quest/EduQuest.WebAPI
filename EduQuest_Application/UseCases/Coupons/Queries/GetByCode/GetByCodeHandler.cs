using AutoMapper;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Coupons.Queries.GetByCode;

internal class GetByCodeHandler : IRequestHandler<GetByCodeCommand, APIResponse>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;
    private const string key = "name";
    private const string value = "coupon";
    public GetByCodeHandler(ICouponRepository couponRepository, IMapper mapper)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetByCodeCommand request, CancellationToken cancellationToken)
    {
       var coupon = await _couponRepository.GetCouponByCode(request.Code);
        if (coupon == null)
        {
            GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, 
                MessageCommon.GetFailed, MessageCommon.NotFound, key, value);
        }
        CouponResponse response = _mapper.Map<CouponResponse>(coupon);
        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully,
            response, key, value);
    }
}
