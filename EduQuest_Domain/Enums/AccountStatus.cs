using System.ComponentModel;

namespace EduQuest_Domain.Enums;

public enum AccountStatus
{
    [Description("Active")]
    Active = 1,
    [Description("Pending")]
    Pending = 2,
    [Description("Blocked")]
    Blocked = 3,
    [Description("Rejected")]
    Rejected = 4

}