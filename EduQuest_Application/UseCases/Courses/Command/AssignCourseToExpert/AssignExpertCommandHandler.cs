using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;

namespace EduQuest_Application.UseCases.Courses.Command.AssignCourseToExpert;

public class AssignExpertCommandHandler : IRequestHandler<AssignExpertCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;

	public AssignExpertCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, ICourseRepository courseRepository)
	{
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
		_courseRepository = courseRepository;
	}

	public async Task<APIResponse> Handle(AssignExpertCommand request, CancellationToken cancellationToken)
    {
        var existUser = await _userRepository.GetById(request.AssignTo);
        if (existUser == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.LoginFailed, MessageCommon.LoginFailed, "otp", $"User with ID {request.AssignTo}");

		}

        var existCourse = await _courseRepository.GetById(request.CourseId);
        if (existCourse == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", $"Course with ID {request.CourseId}");
        }

        existCourse.AssignTo = request.AssignTo;
        await _courseRepository.Update(existCourse);
        await _unitOfWork.SaveChangesAsync();

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.AssignExpert, existUser, "name", $"User with ID {request.AssignTo}");
    }
}
