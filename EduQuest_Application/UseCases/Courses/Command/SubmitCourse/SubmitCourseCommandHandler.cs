using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Domain.Enums;

namespace EduQuest_Application.UseCases.Courses.Command.SubmitCourse;

public class SubmitCourseCommandHandler : IRequestHandler<SubmitCourseCommand, APIResponse>
{
    private readonly ICourseRepository _courseRepository;

    public SubmitCourseCommandHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
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
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.SubmitSuccessfully, null, "name", result.Title);
        }
        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateFailed, null, "name", result.Title);
    }
}
