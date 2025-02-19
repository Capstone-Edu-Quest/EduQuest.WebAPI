

using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Coupons.Commands.UpdateCourseCoupons;

public class UpdateCourseCouponHandler : IRequestHandler<UpdateCourseCouponCommand, APIResponse>
{
    public Task<APIResponse> Handle(UpdateCourseCouponCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
