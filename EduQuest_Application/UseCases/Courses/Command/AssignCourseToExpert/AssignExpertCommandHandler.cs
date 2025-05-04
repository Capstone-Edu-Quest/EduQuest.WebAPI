using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Notification;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Command.AssignCourseToExpert;

public class AssignExpertCommandHandler : IRequestHandler<AssignExpertCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IFireBaseRealtimeService _fireBaseRealtimeService;

    public AssignExpertCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, ICourseRepository courseRepository, IFireBaseRealtimeService fireBaseRealtimeService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _fireBaseRealtimeService = fireBaseRealtimeService;
    }

    public async Task<APIResponse> Handle(AssignExpertCommand request, CancellationToken cancellationToken)
    {
        var existUser = await _userRepository.GetById(request.AssignTo);
        if (existUser == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.NotFound, MessageCommon.NotFound, "name", request.AssignTo);

		}

        var existCourse = await _courseRepository.GetById(request.CourseId);
        if (existCourse == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", request.CourseId);
        }

        existCourse.AssignTo = request.AssignTo;
        await _courseRepository.Update(existCourse);
        await _unitOfWork.SaveChangesAsync();

        await _fireBaseRealtimeService.PushNotificationAsync(new NotificationDto
        {
            userId = existUser.Id,
            Receiver = existUser.Id,
            Content = NotificationMessage.A_STAFF_HAS_ASSIGN_COURSE_TO_YOU,
            Url = "/courses-manage/approval",
            Values = new Dictionary<string, string>
            {
                { "course", existCourse.Title },
            }
        });
        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.AssignExpert, existUser, "name", request.AssignTo);
    }
}
