using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.UserStatistics;

public class UserGraphData
{
    public int TotalActiveUser { get; set; } = 0;
    public int TotalProUser { get; set; } = 0;
    public int TotalUser {  get; set; } = 0;
    public string Date { get; set; } = string.Empty;
}
