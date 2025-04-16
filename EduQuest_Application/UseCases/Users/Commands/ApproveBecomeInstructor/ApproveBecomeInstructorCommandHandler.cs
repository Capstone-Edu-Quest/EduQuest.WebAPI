using AutoMapper;
using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Application.DTO.Response.Profiles;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Notification;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;

namespace EduQuest_Application.UseCases.Users.Commands.ApproveBecomeInstructor;

public class ApproveBecomeInstructorCommandHandler : IRequestHandler<ApproveBecomeInstructorCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFireBaseRealtimeService _fireBaseRealtimeService;

    public ApproveBecomeInstructorCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IFireBaseRealtimeService fireBaseRealtimeService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _fireBaseRealtimeService = fireBaseRealtimeService;
    }

    public async Task<APIResponse> Handle(ApproveBecomeInstructorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.UserId);

        user.Status = request.isApprove ? AccountStatus.Active.ToString() : AccountStatus.Rejected.ToString();

        await _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        string message = request.isApprove
            ? Constants.NotificationMessage.BECOME_INSTRUCTOR_APPROVED
            : Constants.NotificationMessage.BECOME_INSTRUCTOR_REJECTED;


        await _fireBaseRealtimeService.PushNotificationAsync(new NotificationDto
        {
            userId = user.Id,
            Receiver = user.Id,
            Content = "NEW_COURSE",
            Url = "",
            Values = new Dictionary<string, string>
            {
                { "courseId", "course-101" },
            }
        });

        
        return GeneralHelper.CreateSuccessResponse(
           HttpStatusCode.OK,
           Constants.MessageCommon.ApproveSuccessfully,
           null,
           "name",
           user.Username
       );
    }
}
