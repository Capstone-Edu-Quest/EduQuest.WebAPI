

using static EduQuest_Domain.Constants.Constants;
using System.ComponentModel.DataAnnotations;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Coupons;

public class CreatePlatformCouponRequest : IMapFrom<Coupon>, IMapTo<Coupon>
{
    [Required(ErrorMessage = MessageError.ValueRequired)]
    public double DiscountValue { get; set; }

    public string? CustomeCode { get; set; }


    [Required(ErrorMessage = MessageError.ValueRequired)]
    public DateTime ExpireAt { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public int Usage { get; set; }
}
