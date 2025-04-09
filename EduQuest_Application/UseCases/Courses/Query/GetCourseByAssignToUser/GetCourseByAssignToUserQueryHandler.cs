using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Courses.Queries.SearchCourse;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using static StackExchange.Redis.Role;
using System.Net;

namespace EduQuest_Application.UseCases.Courses.Query.GetCourseByAssignToUser;

public class GetCourseByAssignToUserQueryHandler : IRequestHandler<GetCourseByAssignToUserQuery, APIResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public GetCourseByAssignToUserQueryHandler(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetCourseByAssignToUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _courseRepository.GetCoursesByAssignToAsync(request.expertId);
        var mappedResult = _mapper.Map<List<CourseExpertResponseDto>>(result);
        return GeneralHelper.CreateSuccessResponse(
                    HttpStatusCode.OK,
                    MessageCommon.CreateSuccesfully,
                    mappedResult,
                    "name",
                    "courses"
                );
    }
}
