using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Notification;
using EduQuest_Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Infrastructure.ExternalServices.Quartz.Certificates;

internal class ProvideCertificate : IJob
{
    private readonly ISchedulerFactory _scheduler;
    private readonly ILearnerRepository _learnerRepository;
    private readonly ICertificateRepository _certificateRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IFireBaseRealtimeService _fireBaseRealtimeService;
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly ILogger<ProvideCertificate> _logger;

    public ProvideCertificate(ISchedulerFactory scheduler, ILearnerRepository learnerRepository,
        ICertificateRepository certificateRepository, ICourseRepository courseRepository,
        IFireBaseRealtimeService fireBaseRealtimeService, ILogger<ProvideCertificate> logger, ILearningPathRepository learningPathRepository)
    {
        _scheduler = scheduler;
        _learnerRepository = learnerRepository;
        _certificateRepository = certificateRepository;
        _courseRepository = courseRepository;
        _fireBaseRealtimeService = fireBaseRealtimeService;
        _logger = logger;
        _learningPathRepository = learningPathRepository;
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
            var IsHavingCertificate = await _certificateRepository.GetCertificatesByCourseIdAndUserId(entry.UserId, entry.CourseId);
            if (IsHavingCertificate)
            {
                continue;
            }
            await _learningPathRepository.UpdateLeanringPathIsComplete(entry.UserId);
            var certId = Guid.NewGuid().ToString();
            var newCertificate = new Certificate
            {
                Id = certId,
                UserId = entry.UserId,
                CourseId = entry.CourseId,
                CreatedAt = DateTime.UtcNow.ToUniversalTime(),
                UpdatedAt = DateTime.UtcNow.ToUniversalTime(),
                Title = "",
                Url = "",
            };

            //send notification after completed course
            var courseName = await _courseRepository.GetById(entry.CourseId);
            //await _fireBaseRealtimeService.PushNotificationAsync(
            //                  new NotificationDto
            //                  {
            //                      userId = entry.UserId,
            //                      Content = NotificationMessage.COMPLETED_COURSE_SUCCESSFULLY,
            //                      Receiver = entry.UserId,
            //                      Url = $"/c/{certId}",
            //                      Values = new Dictionary<string, string>
            //                      {
            //                            { "certificate", courseName.Title}
            //                      }
            //                  }
            //              );



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