using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.UserStatistics;

public class AdminDasboardUsers
{
    public int TotalUsers {  get; set; } = 0;

    public List<UserGraphData> GraphData { get; set; } = new List<UserGraphData>();
    public int MonthlyActiveUsers { get; set; } = 0;
    public int ThisMonthNewUsers { get; set; } = 0;
}
