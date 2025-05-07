
using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Query.GetCourseByStatus;

public class GetCourseByStatusQueryHandler : IRequestHandler<GetCourseByStatusQuery, APIResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICourseStatisticRepository _courseStatisticRepository;
    private readonly IMapper _mapper;

    public GetCourseByStatusQueryHandler(ICourseRepository courseRepository, IMapper mapper, IUserRepository userRepository, ICourseStatisticRepository courseStatisticRepository)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _courseStatisticRepository = courseStatisticRepository;
    }

    public async Task<APIResponse> Handle(GetCourseByStatusQuery request, CancellationToken cancellationToken)
    {
        var exists = await _courseRepository.GetCourseByStatus(request.Status);


        var listCourseResponse = _mapper.Map<List<CourseSearchResponse>>(exists.OrderByDescending(x => x.UpdatedAt)); //Chưa check Discount Price
		

		foreach (var course in listCourseResponse)
        {
            var user = await _userRepository.GetById(course.CreatedBy);
            course.Author = user!.Username!;

            var expert = await _userRepository.GetById(course.ExpertId);
            if (expert != null)
            {
                course.ExpertName = expert!.Username!;
            }
            

            var courseSta = await _courseStatisticRepository.GetByCourseId(course.Id);
            if (courseSta != null)
            {
                course.TotalLesson = (int)courseSta.TotalLesson;
                course.TotalReview = (int)courseSta.TotalReview;
                course.Rating = (int)courseSta.Rating;
                course.TotalTime = (int)courseSta.TotalTime;
            }

		}

        int totalItem = listCourseResponse.Count;
        var listPaged = listCourseResponse.Skip((request.PageNo - 1) * request.EachPage)
                                        .Take(request.EachPage)
                                        .ToList();
        var result = new PagedList<CourseSearchResponse>()
        {
            TotalItems = totalItem,
            Items = listPaged
        };


        return new APIResponse
        {
            IsError = false,
            Payload = result,
            Errors = null,
            Message = new MessageResponse
            {
                content = MessageCommon.GetSuccesfully,
                values = new Dictionary<string, string>() { { "name", "courses" } }
            }
        };
    }
}
