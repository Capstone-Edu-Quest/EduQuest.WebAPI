using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using Quartz;

namespace EduQuest_Infrastructure.ExternalServices.Quartz.Quests;

public class UpdateAllUserQuest : IJob
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IUserQuestRepository _userQuestRepository;
    private readonly IQuestRepository _questRepository;

    public UpdateAllUserQuest(ISchedulerFactory schedulerFactory, IUserQuestRepository userQuestRepository, IQuestRepository questRepository)
    {
        _schedulerFactory = schedulerFactory;
        _userQuestRepository = userQuestRepository;
        _questRepository = questRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // get scheduler
        IScheduler scheduler = await _schedulerFactory.GetScheduler();

        // get job and trigge
        IJobDetail currentJob = context.JobDetail;
        ITrigger currentTrigger = context.Trigger;
        Quest quest = await _questRepository.GetQuestById(currentJob.Key.ToString().Substring(8));
        bool result = await _userQuestRepository.UpdateAllUserQuest(quest!);
        Console.WriteLine("Task run: Update All UserQuests!");

        // delete job and trigger
        await scheduler.DeleteJob(currentJob.Key);
        await scheduler.UnscheduleJob(currentTrigger.Key);

        Console.WriteLine($"DeleteJob Update All UserQuests: {currentJob.Key}");
    }
}
