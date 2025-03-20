using EduQuest_Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Models.Subscriptions
{
    public class RolePackageNumbersDto
    {
		public Dictionary<GeneralEnums.ConfigEnum, decimal?> Free { get; set; }
		public Dictionary<GeneralEnums.ConfigEnum, decimal?> Pro { get; set; }
	}
}
