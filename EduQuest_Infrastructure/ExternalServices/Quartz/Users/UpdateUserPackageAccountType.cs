using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Repository;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.ExternalServices.Quartz.Users
{
	public class UpdateUserPackageAccountType : IJob
	{
		private readonly ISchedulerFactory _schedulerFactory;
		private readonly IUserRepository _userRepository;

		public UpdateUserPackageAccountType(ISchedulerFactory schedulerFactory, IUserRepository userRepository)
		{
			_schedulerFactory = schedulerFactory;
			_userRepository = userRepository;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			IScheduler scheduler = await _schedulerFactory.GetScheduler();

			// get job and trigge
			IJobDetail currentJob = context.JobDetail;
			ITrigger currentTrigger = context.Trigger;

			bool result = await _userRepository.UpdateUserPackageAccountType(currentJob.Key.ToString().Substring(8));
			Console.WriteLine("Task run: Update user package account type!");

			// delete job and trigger
			await scheduler.DeleteJob(currentJob.Key);
			await scheduler.UnscheduleJob(currentTrigger.Key);

			Console.WriteLine($"DeleteJob Update user package account type: {currentJob.Key}");
		}
	}
}
