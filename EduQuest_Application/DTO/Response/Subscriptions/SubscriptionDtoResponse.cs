using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Subscriptions
{
	public class SubscriptionDtoResponse: IMapFrom<Subscription>, IMapTo<Subscription>
	{
		public string Package { get; set; }
		public decimal Monthly { get; set; }
		public decimal Yearly { get; set; }
		public List<BenefitDtoResponse> Benefits { get; set; }
	}
}
