using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Subscriptions
{
	public class SubscriptionRequest
	{
        public int RoleId { get; set; }
        public int PackageEnum { get; set; }
        public int ConfigEnum { get; set; }
        public decimal Value { get; set; }
    }
}
