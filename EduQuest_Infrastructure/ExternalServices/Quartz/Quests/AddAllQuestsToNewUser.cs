using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Repository;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.ExternalServices.Quartz.Quests;

public class AddAllQuestsToNewUser : IJob
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IUserQuestRepository _userQuestRepository;

    public AddAllQuestsToNewUser(ISchedulerFactory schedulerFactory, IUserQuestRepository userQuestRepository)
    {
        _schedulerFactory = schedulerFactory;
        _userQuestRepository = userQuestRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // get scheduler
        IScheduler scheduler = await _schedulerFactory.GetScheduler();

        // get job and trigge
        IJobDetail currentJob = context.JobDetail;
        ITrigger currentTrigger = context.Trigger;
        bool result = await _userQuestRepository.AddQuestsToNewUser(currentJob.Key.ToString().Substring(8));
        Console.WriteLine($"Task run: Add All Quests To New User! IsCompleted: {result}");

        // delete job and trigger
        await scheduler.DeleteJob(currentJob.Key);
        await scheduler.UnscheduleJob(currentTrigger.Key);

        Console.WriteLine($"DeleteJob Add New Quest To All UserQuest: {currentJob.Key}");
    }
}
