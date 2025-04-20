using EduQuest_Application.Abstractions.Email;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.ExternalServices.Quartz.Certificates;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<ResetDailyQuest> _logger;
    private readonly IEmailService _emailService;
    public ResetDailyQuest(IUserQuestRepository userQuestRepository, ILogger<ResetDailyQuest> logger, IEmailService emailService)
    {
        _userQuestRepository = userQuestRepository;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        bool result = await _userQuestRepository.ResetDailyQuests();
        await _emailService.SendEmailVerifyAsync("Reset Daily Quests Task", "minhduylongthuan@gmail.com",
            "Task run: Reset Reset Daily Quests!", "OTP", "PATH", "logo");
        _logger.Log(LogLevel.Information, "Start running Reset Daily Quest job");
    }
}
