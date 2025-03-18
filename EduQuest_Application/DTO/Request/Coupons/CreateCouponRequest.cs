

using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.DTO.Request.Coupons;

public class CreateCouponRequest : IMapFrom<Coupon>, IMapTo<Coupon>
{
    [Required(ErrorMessage = MessageError.ValueRequired)]
    [Range(1, 100, ErrorMessage = "Discount ranged from 1 to 100")]
    public double Discount { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public string? Code { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public string? Description { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public DateTime StartTime { get; set; }

    public DateTime? ExpireTime { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public int AllowUsagePerUser { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public int Limit { get; set; }

/*    public List<string>? WhiteListCourseIds { get; set; }
    public List<string>? WhiteListUserIds { get; set; }*/
}
