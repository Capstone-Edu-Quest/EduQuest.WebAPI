
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Domain.Repository;
using Microsoft.Extensions.Logging;
using Quartz;

namespace EduQuest_Infrastructure.ExternalServices.Quartz.Users;

public class InitializeLeaderboard : IJob
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IUserMetaRepository _userMetaRepository;
    private readonly IRedisCaching _redis;
    private readonly ILogger<InitializeLeaderboard> _logger;

    public InitializeLeaderboard(ISchedulerFactory schedulerFactory, IUserMetaRepository userMetaRepository, IRedisCaching redis,
        ILogger<InitializeLeaderboard> logger)
    {
        _schedulerFactory = schedulerFactory;
        _userMetaRepository = userMetaRepository;
        _redis = redis;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        IScheduler scheduler = await _schedulerFactory.GetScheduler();

        // get job and trigge
        IJobDetail currentJob = context.JobDetail;
        ITrigger currentTrigger = context.Trigger;
        var userMetas = await _userMetaRepository.GetLeaderboardData();


        _logger.Log(LogLevel.Information, "Task run: Reload leaderboard data!");
        foreach (var userMeta in userMetas)
        {
            await _redis.AddToSortedSetAsync("leaderboard:season1", userMeta.UserId, userMeta.TotalStudyTime.Value);
        };
        _logger.Log(LogLevel.Information, "Completed task: Reload leaderboard data!");

        // delete job and trigger
        await scheduler.DeleteJob(currentJob.Key);
        await scheduler.UnscheduleJob(currentTrigger.Key);
        _logger.Log(LogLevel.Information, $"DeleteJob Update user package account type: {currentJob.Key}");
    }
}
