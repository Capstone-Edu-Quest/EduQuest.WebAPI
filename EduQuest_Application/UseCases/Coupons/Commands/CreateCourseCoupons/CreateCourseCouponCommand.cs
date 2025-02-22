using EduQuest_Application.DTO.Request.Coupons;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Coupons.Commands.CreateCourseCoupons;

public class CreateCourseCouponCommand : IRequest<APIResponse>
{
    public string UserId {  get; set; }
    public CreateCouponRequest CreateCouponRequest { get; set; }

    public CreateCourseCouponCommand(string userId, CreateCouponRequest createCouponRequest)
    {
        UserId = userId;
        CreateCouponRequest = createCouponRequest;
    }
}
