using EduQuest_Domain.Repository;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.ExternalServices.Quartz.Quests;

public class ResetDailyQuest : IJob
{
    private readonly IUserQuestRepository _userQuestRepository;

    public ResetDailyQuest(IUserQuestRepository userQuestRepository)
    {
        _userQuestRepository = userQuestRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        bool result = await _userQuestRepository.ResetDailyQuests();
        Console.WriteLine("Task run: Reset All Daily Quests!");
    }
}
