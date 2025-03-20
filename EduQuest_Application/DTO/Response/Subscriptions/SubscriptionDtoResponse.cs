using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Subscriptions
{
    public class SubscriptionDtoResponse
	{
        public string Name { get; set; }
        public Dictionary<int, RolePackageDto> Data { get; set; }
    }
}
