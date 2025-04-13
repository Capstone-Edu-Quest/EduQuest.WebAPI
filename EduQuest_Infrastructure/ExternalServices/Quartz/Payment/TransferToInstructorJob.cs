using EduQuest_Application.UseCases.Transactions.Command.TransfertoAllInstructor;
using MediatR;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.ExternalServices.Quartz.Payment
{
	public class TransferToInstructorJob : IJob
	{
		private readonly IMediator _mediator;

		public TransferToInstructorJob(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			var transactionId = context.JobDetail.JobDataMap.GetString("TransactionId");

			if (!string.IsNullOrEmpty(transactionId))
			{
				var command = new TransfertoAllInstructorCommand
				{
					TransactionId = transactionId
				};
				await _mediator.Send(command);
			}
		}
	}
}
