using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Quest;

public class ClaimRewardResponse
{
    public int? Gold {  get; set; }
    public int? Exp { get; set; }
    public string? Coupon { get; set; }
    public int? Booster { get; set; }
    public string? Item { get; set; }
}
