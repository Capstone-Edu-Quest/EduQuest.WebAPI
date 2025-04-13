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
		private readonly ISchedulerFactory _schedulerFactory;

		public TransferToInstructorJob(IMediator mediator, ISchedulerFactory schedulerFactory)
		{
			_mediator = mediator;
			_schedulerFactory = schedulerFactory;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			// get scheduler
			IScheduler scheduler = await _schedulerFactory.GetScheduler();

			// get job and trigger
			IJobDetail currentJob = context.JobDetail;
			ITrigger currentTrigger = context.Trigger;

			var transactionId = currentJob.Key.ToString().Substring(8);

			if (!string.IsNullOrEmpty(transactionId))
			{
				var command = new TransfertoAllInstructorCommand
				{
					TransactionId = transactionId
				};
				await _mediator.Send(command);
			}

			// delete job and trigger
			await scheduler.DeleteJob(currentJob.Key);
			await scheduler.UnscheduleJob(currentTrigger.Key);

			Console.WriteLine($"Transfer to Instructor in TransactionId: {currentJob.Key}");
		}
	}
}
