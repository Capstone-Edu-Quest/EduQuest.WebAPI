using EduQuest_Application.Abstractions.Email;
using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Notification;
using EduQuest_Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using static EduQuest_Domain.Constants.Constants;
using static Grpc.Core.Metadata;

namespace EduQuest_Infrastructure.ExternalServices.Quartz.Certificates;

public class UpdateLearningPathCourseDueDate : IJob
{
    private ILearningPathRepository _learningPathRepository;
    private readonly IFireBaseRealtimeService _fireBaseRealtimeService;
    private readonly IEmailService _emailService;
    private ILogger<UpdateLearningPathCourseDueDate> _logger;

    public UpdateLearningPathCourseDueDate(ILearningPathRepository learningPathRepository, 
        ILogger<UpdateLearningPathCourseDueDate> logger,
        IFireBaseRealtimeService fireBaseRealtimeService,
        IEmailService emailService)
    {
        _learningPathRepository = learningPathRepository;
        _logger = logger;
        _fireBaseRealtimeService = fireBaseRealtimeService;
        _emailService = emailService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.Log(LogLevel.Information, "Update Learning Path Course DueDate Job");
        await _learningPathRepository.UpdateLearningPathCourseDueDate();
        var overdue = await _learningPathRepository.GetOverDueLeanringPath();
        if (overdue != null)
        {
            
            foreach (var item in overdue)
            {
                //await _fireBaseRealtimeService.PushNotificationAsync(
                //              new NotificationDto
                //              {
                //                  userId = item.UserId,
                //                  Content = NotificationMessage.LEARNING_PATH_OVERDUE,
                //                  Receiver = item.UserId,
                //                  Url = "",
                //                  Values = new Dictionary<string, string>
                //                  {
                //                        { "learning_path", item.LearningPath.Name }
                //                  }
                //              }
                //          );
                var user = item.User;
                await _emailService.SendEmailWarningLearningPathOverDueAsync(
                    "LEARNING PATH OVERDUE WARNING EMAIL", user.Email!, item.LearningPath.Name, item.LearningPathId,
                    item.Course.Title,
                    "./template/LearningPathDueDateWarning.cshtml",
                    "./template/LOGO 3.png");
                await _learningPathRepository.MarkAsReminded(item.Id);
            }
        }
    }

}