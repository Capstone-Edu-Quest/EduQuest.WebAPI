using System.ComponentModel;

namespace EduQuest_Domain.Enums;

public enum AccountStatus
{
    [Description("Active")]
    Active = 1,
    [Description("Blocked")]
    Blocked = 3
}