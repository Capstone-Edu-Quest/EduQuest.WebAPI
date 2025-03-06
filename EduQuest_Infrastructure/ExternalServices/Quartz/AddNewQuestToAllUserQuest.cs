using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using Quartz;

namespace EduQuest_Infrastructure.ExternalServices.Quartz;

/*public class AddNewQuestToAllUserQuest : IJob
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IUserQuestRepository _userQuestRepository;
    private readonly IQuestRepository _questRepository;

    public AddNewQuestToAllUserQuest(ISchedulerFactory schedulerFactory, IUserQuestRepository userQuestRepository,
        IQuestRepository questRepository)
    {
        _schedulerFactory = schedulerFactory;
        _userQuestRepository = userQuestRepository;
        _questRepository = questRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // get scheduler
        IScheduler scheduler = await _schedulerFactory.GetScheduler();

        // get job and trigger
        IJobDetail currentJob = context.JobDetail;
        ITrigger currentTrigger = context.Trigger;
        Quest quest = await _questRepository.GetQuestById(currentJob.Key.ToString().Substring(6));
        bool result = await _userQuestRepository.AddNewQuestToAllUseQuest(quest!);
        Console.WriteLine("Task run: Event status changed to Ongoing!");

        // delete job and trigger
        await scheduler.DeleteJob(currentJob.Key);
        await scheduler.UnscheduleJob(currentTrigger.Key);

        Console.WriteLine($"DeleteJob Event status changed to Ongoing Job: {currentJob.Key}");
    }
}*/
