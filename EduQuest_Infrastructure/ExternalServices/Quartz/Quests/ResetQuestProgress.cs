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

public class ResetQuestProgress : IJob
{
    private readonly IUserQuestRepository _userQuestRepository;

    public ResetQuestProgress(IUserQuestRepository userQuestRepository)
    {
        _userQuestRepository = userQuestRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        bool result = await _userQuestRepository.ResetQuestProgress();
        Console.WriteLine("Task run: Reset All UserQuests Progress!");
    }
}
