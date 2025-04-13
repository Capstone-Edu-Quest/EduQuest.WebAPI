

using EduQuest_Application.ExternalServices.QuartzService;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Infrastructure.ExternalServices.Quartz.Payment;
using EduQuest_Infrastructure.ExternalServices.Quartz.Quests;
using EduQuest_Infrastructure.ExternalServices.Quartz.Users;
using Quartz;

namespace EduQuest_Infrastructure.ExternalServices.Quartz;

public class QuartzService : IQuartzService
{
    private readonly ISchedulerFactory _schedulerFactory;

    public QuartzService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task AddNewQuestToAllUser(string questId)
    {
        var jobKey = new JobKey(questId);
        IScheduler scheduler = await _schedulerFactory.GetScheduler();

        DateTime now = DateTime.Now;
        DateTime scheduleTime;

        // current time is before 5.AM
        if (now.TimeOfDay < new TimeSpan(5, 0, 0))
        {
            // schedule at  5.AM today
            scheduleTime = new DateTime(now.Year, now.Month, now.Day, 5, 0, 0);
        }
        else
        {
            // schedule at  5.AM tomorrow
            scheduleTime = new DateTime(now.Year, now.Month, now.Day, 5, 0, 0).AddDays(1);
        }

        IJobDetail job = JobBuilder.Create<AddNewQuestToAllUserQuest>()
        .WithIdentity(jobKey)
        .Build();
        var newTrigger =
            TriggerBuilder.Create().ForJob(jobKey)
            .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(DateTime.Now.AddSeconds(5))))
            .Build();
        await scheduler.ScheduleJob(job, newTrigger);
        Console.WriteLine($"ScheduleJob: AddNewQuestToAllUser with id {jobKey} at {DateTime.Now.AddSeconds(5)}.");
    }

    public async Task UpdateAllUserQuest(string questId)
    {
        var jobKey = new JobKey(questId);
        IScheduler scheduler = await _schedulerFactory.GetScheduler();
        IJobDetail job = JobBuilder.Create<UpdateAllUserQuest>()
        .WithIdentity(jobKey)
        .Build();
        var newTrigger =
            TriggerBuilder.Create().ForJob(jobKey)
            .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(DateTime.Now.AddSeconds(15))))
            .Build();
        await scheduler.ScheduleJob(job, newTrigger);
        Console.WriteLine($"ScheduleJob: Update All UserQuests with id {jobKey}.");
    }

	public async Task UpdateUserPackageAccountType(string userId)
	{
		var jobKey = new JobKey(userId);
		IScheduler scheduler = await _schedulerFactory.GetScheduler();
		IJobDetail job = JobBuilder.Create<UpdateUserPackageAccountType>()
		.WithIdentity(jobKey)
		.Build();
		var newTrigger =
			TriggerBuilder.Create().ForJob(jobKey)
			.WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(DateTime.Now.AddMinutes(5))))
			.Build();
		await scheduler.ScheduleJob(job, newTrigger);
		Console.WriteLine($"ScheduleJob: Update user package account type with id {jobKey}.");
	}

    public async Task AddAllQuestsToNewUser(string userId)
    {
        var jobKey = new JobKey(userId);
        IScheduler scheduler = await _schedulerFactory.GetScheduler();

        IJobDetail job = JobBuilder.Create<AddAllQuestsToNewUser>()
        .WithIdentity(jobKey)
        .Build();
        var newTrigger =
            TriggerBuilder.Create().ForJob(jobKey)
            .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(DateTime.Now.AddSeconds(15))))
            .Build();
        await scheduler.ScheduleJob(job, newTrigger);
        Console.WriteLine($"ScheduleJob: Add All Quests To New User with id {jobKey}.");
    }

	public async Task TransferToInstructor(string TransactionId)
	{
		var jobKey = new JobKey(TransactionId);
		IScheduler scheduler = await _schedulerFactory.GetScheduler();
		IJobDetail job = JobBuilder.Create<TransferToInstructorJob>()
		.WithIdentity(jobKey)
		.Build();
		var newTrigger =
			TriggerBuilder.Create().ForJob(jobKey)
			.WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(DateTime.Now.AddMinutes(1))))
			.Build();
		await scheduler.ScheduleJob(job, newTrigger);
		Console.WriteLine($"ScheduleJob: Transfer to all instructors with id {jobKey}.");
	}
}
