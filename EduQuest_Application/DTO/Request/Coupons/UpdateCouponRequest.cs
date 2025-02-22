

using static EduQuest_Domain.Constants.Constants;
using System.ComponentModel.DataAnnotations;

namespace EduQuest_Application.DTO.Request.Coupons;

public class UpdateCouponRequest
{
    [Required(ErrorMessage = MessageError.ValueRequired)]
    public double DiscountValue { get; set; }

    //public string? CustomeCode { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public DateTime ExpireAt { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public int AddedUsage { get; set; } = 0;
}
