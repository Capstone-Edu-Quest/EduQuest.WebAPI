using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Payment
{
	public class CreateCheckoutRequest
	{
		public string? CartId { get; set; }
		public string? CouponCode { get; set; }
		public int? ConfigEnum { get; set; }
		public string? SuccessUrl { get; set; }
		public string? CancelUrl { get; set; }
	}
}
