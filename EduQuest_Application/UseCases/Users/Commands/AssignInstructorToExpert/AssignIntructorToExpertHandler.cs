using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Users.Commands.AssignInstructorToExpert;

public class AssignIntructorToExpertHandler : IRequestHandler<AssignIntructorToExpert, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public AssignIntructorToExpertHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(AssignIntructorToExpert request, CancellationToken cancellationToken)
    {
        var existUser = await _userRepository.GetById(request.InstructorId);
        if (existUser == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.LoginFailed, MessageCommon.LoginFailed, "otp", $"User with ID {request.AssignTo}");

        }

        var existExpert = await _userRepository.GetById(request.AssignTo);
        if (existUser == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.LoginFailed, MessageCommon.LoginFailed, "otp", $"User with ID {request.AssignTo}");

        }


        existUser.AssignToExpertId = request.AssignTo;
        await _userRepository.Update(existUser);
        await _unitOfWork.SaveChangesAsync();

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.AssignExpert, existUser, "name", $"User with ID {request.AssignTo}");

    }
}
