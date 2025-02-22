

using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.DTO.Request.Coupons;

public class CreateCouponRequest : IMapFrom<Coupon>, IMapTo<Coupon>
{
    [Required(ErrorMessage = MessageError.ValueRequired)]
    public double DiscountValue { get; set; }

    public string? CustomeCode { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public string CourseId { get; set; } = string.Empty;

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public DateTime ExpireAt { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public int Usage { get; set; }
}
