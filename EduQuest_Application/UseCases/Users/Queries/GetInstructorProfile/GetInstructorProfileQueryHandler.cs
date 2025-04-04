using AutoMapper;
using EduQuest_Application.DTO.Response.Profiles;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetInstructorProfile;

public class GetInstructorProfileQueryHandler : IRequestHandler<GetInstructorProfileQuery, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public GetInstructorProfileQueryHandler(IUserRepository userRepository, ICourseRepository courseRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetInstructorProfileQuery request, CancellationToken cancellationToken)
    {
        // 1. Lấy user (instructor)
        var user = await _userRepository.GetById(request.userId);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.NotFound, Constants.MessageCommon.NotFound, "name", request.userId ?? "");
        }

        var courses = await _courseRepository.GetCoursesByInstructorIdAsync(user.Id);

        var courseDtos = _mapper.Map<List<CourseInstructorProfileDto>>(courses);

        foreach (var coursedto in courseDtos)
        {
            coursedto.Author = user.Username;
        }

        int totalLearners = courses.Sum(c => c.CourseLearners?.Count ?? 0);
        int totalReviews = courses.Sum(c => c.Feedbacks?.Count ?? 0);
        //double averageReview = courses.Any()
        //    ? courses.Average(c => c.CourseStatistic?.Rating ?? 0)
        //    : 0;

        var instructorDto = _mapper.Map<InstructorProfileDto>(user);
        instructorDto.Courses = courseDtos;
        instructorDto.TotalLearners = totalLearners;
        instructorDto.TotalReviews = totalReviews;
        //instructorDto.AvarageReviews = (int)averageReview;

        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.GetSuccesfully, instructorDto, "name", instructorDto.Email ?? "");
    }

}
