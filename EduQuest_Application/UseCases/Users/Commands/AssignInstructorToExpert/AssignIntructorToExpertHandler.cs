using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Domain.Models.Notification;

namespace EduQuest_Application.UseCases.Users.Commands.AssignInstructorToExpert;

public class AssignIntructorToExpertHandler : IRequestHandler<AssignIntructorToExpert, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IFireBaseRealtimeService _fireBaseRealtimeService;

    public AssignIntructorToExpertHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IFireBaseRealtimeService fireBaseRealtimeService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _fireBaseRealtimeService = fireBaseRealtimeService;
    }

    public async Task<APIResponse> Handle(AssignIntructorToExpert request, CancellationToken cancellationToken)
    {
        var existUser = await _userRepository.GetById(request.InstructorId);
        if (existUser == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.NotFound, MessageCommon.NotFound, "name", request.InstructorId);

        }

        var existExpert = await _userRepository.GetById(request.AssignTo);
        if (existExpert == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.NotFound, MessageCommon.NotFound, "name", request.AssignTo);

        }


        existUser.AssignToExpertId = request.AssignTo;
        await _userRepository.Update(existUser);
        await _unitOfWork.SaveChangesAsync();
        await _fireBaseRealtimeService.PushNotificationAsync(new NotificationDto
        {
            userId = existExpert.Id,
            Receiver = existExpert.Id,
            Content = NotificationMessage.A_STAFF_HAS_ASSIGN_INSTRUCTOR_TO_YOU,
            Url = "",
            Values = new Dictionary<string, string>
            {
                { "name", existUser.Username },
            }
        });
        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.AssignExpert, existUser, "name", request.AssignTo);

    }
}
