

using EduQuest_Application.ExternalServices.QuartzService;
using EduQuest_Application.Helper;
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
        IJobDetail job = JobBuilder.Create<AddNewQuestToAllUserQuest>()
        .WithIdentity(jobKey)
        .Build();
        var newTrigger =
            TriggerBuilder.Create().ForJob(jobKey)
            .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(DateTime.Now.AddMinutes(1))))
            .Build();
        await scheduler.ScheduleJob(job, newTrigger);
        Console.WriteLine($"ScheduleJob:  AddNewQuestToAllUser with id {jobKey}");
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
            .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(DateTime.Now.AddMinutes(1))))
            .Build();
        await scheduler.ScheduleJob(job, newTrigger);
        Console.WriteLine($"ScheduleJob:  Update All UserQuests with id {jobKey}");
    }
}
