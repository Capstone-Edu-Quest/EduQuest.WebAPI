using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Notification;
using EduQuest_Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Infrastructure.ExternalServices.Quartz.Certificates;

public class UpdateLearningPathCourseDueDate : IJob
{
    private ILearningPathRepository _learningPathRepository;
    private ILogger<UpdateLearningPathCourseDueDate> _logger;

    public UpdateLearningPathCourseDueDate(ILearningPathRepository learningPathRepository,
        ILogger<UpdateLearningPathCourseDueDate> logger)
    {
        _learningPathRepository = learningPathRepository;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.Log(LogLevel.Information, "Update Learning Path Course DueDate Job");
        await _learningPathRepository.UpdateLearningPathCourseDueDate();   
    }

}