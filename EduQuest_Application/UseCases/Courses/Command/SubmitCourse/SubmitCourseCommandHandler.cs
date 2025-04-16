using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Notification;
using EduQuest_Application.Abstractions.Firebase;

namespace EduQuest_Application.UseCases.Courses.Command.SubmitCourse;

public class SubmitCourseCommandHandler : IRequestHandler<SubmitCourseCommand, APIResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFireBaseRealtimeService _notifcation;


    public SubmitCourseCommandHandler(ICourseRepository courseRepository, IFireBaseRealtimeService notifcation)
    {
        _courseRepository = courseRepository;
        _notifcation = notifcation;
    }

    public async Task<APIResponse> Handle(SubmitCourseCommand request, CancellationToken cancellationToken)
    {
        var result = await _courseRepository.GetById(request.courseId);
        if (result == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", "Course");
        }
        if (result.Status == GeneralEnums.StatusCourse.Draft.ToString())
        {
            await _courseRepository.UpdateStatus(GeneralEnums.StatusCourse.Pending.ToString(), request.courseId);
            await _notifcation.PushNotificationAsync(
                new NotificationDto
                {
                    userId = result.CreatedBy,
                    Content = "Submit course successfully",
                    Receiver = result.CreatedBy,
                    Url = BaseUrl.ShopItemUrl,
                }
             );
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.SubmitSuccessfully, null, "name", result.Title);
        }
        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateFailed, null, "name", result.Title);
    }
}
