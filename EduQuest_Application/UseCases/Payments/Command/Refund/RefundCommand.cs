using EduQuest_Application.DTO.Request.Payment;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Payments.Command.Refund
{
	public class RefundCommand : IRequest<APIResponse>
	{
        public RefundRequest Refund { get; set; }

		public RefundCommand(RefundRequest refund)
		{
			Refund = refund;
		}
	}
}
