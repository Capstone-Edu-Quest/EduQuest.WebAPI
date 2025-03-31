using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;

namespace EduQuest_Application.UseCases.Courses.Command.AssignCourseToExpert;

public class AsignExpertCommandHandler : IRequestHandler<AssignExpertCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;

    public AsignExpertCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, ICourseRepository courseRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _courseRepository = courseRepository;
    }

    public async Task<APIResponse> Handle(AssignExpertCommand request, CancellationToken cancellationToken)
    {
        var existUser = await _courseRepository.GetById(request.AssignTo);
        if (existUser == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.LoginFailed, MessageCommon.LoginFailed, "otp", "user");
        }

        var existCourse = await _courseRepository.GetById(request.CourseId);
        if (existCourse == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.LoginFailed, MessageCommon.LoginFailed, "otp", "user");
        }

        existCourse.AssignTo = request.AssignTo;
        //add email feature

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.AssignExpert, null, "name", "user");
    }
}
