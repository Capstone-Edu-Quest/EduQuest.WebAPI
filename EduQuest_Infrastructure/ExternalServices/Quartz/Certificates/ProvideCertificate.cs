using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace EduQuest_Infrastructure.ExternalServices.Quartz.Certificates;

internal class ProvideCertificate : IJob
{
    private readonly ISchedulerFactory _scheduler;
    private readonly ILearnerRepository _learnerRepository;
    private readonly ICertificateRepository _certificateRepository;
    private readonly ILogger<ProvideCertificate> _logger;

    public ProvideCertificate(ISchedulerFactory scheduler, ILearnerRepository learnerRepository, ICertificateRepository certificateRepository, ILogger<ProvideCertificate> logger)
    {
        _scheduler = scheduler;
        _learnerRepository = learnerRepository;
        _certificateRepository = certificateRepository;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        //get scheduler
        IScheduler scheduler = await _scheduler.GetScheduler();
        IJobDetail currentJob = context.JobDetail;

        var finishedLearner = await _learnerRepository.GetFinishedLearner();

        _logger.Log(LogLevel.Information, "Start running provide certificates job");
        var AllNewCertificates = new List<Certificate>();
        foreach (var entry in finishedLearner)
        {
            var newCertificate = new Certificate
            {
                Id = Guid.NewGuid().ToString(),
                UserId = entry.UserId,
                CourseId = entry.CourseId,
                Title = "",
                Url = "",
            };

            AllNewCertificates.Add(newCertificate);
        }

        if (AllNewCertificates.Any())
        {
            await _certificateRepository.BulkCreateAsync(AllNewCertificates);
        }




        // Xóa job sau khi hoàn thành
        //await scheduler.DeleteJob(currentJob.Key);
    }

}