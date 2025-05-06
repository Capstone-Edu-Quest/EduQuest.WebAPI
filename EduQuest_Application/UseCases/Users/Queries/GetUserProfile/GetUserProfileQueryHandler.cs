using AutoMapper;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response.Profiles;
using EduQuest_Application.DTO.Response.Profiles.Heatmap;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Users.Queries.GetUserProfile;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IStudyTimeRepository _studyTimeRepository;
    private readonly ILearnerRepository _learnerRepository;
    private readonly IRedisCaching _redis;
    private readonly IMapper _mapper;

    public GetUserProfileQueryHandler(
        IUserRepository userRepository,
        ICourseRepository courseRepository,
        IStudyTimeRepository studyTimeRepository,
        ILearnerRepository learnerRepository,
        IRedisCaching redis,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _studyTimeRepository = studyTimeRepository;
        _learnerRepository = learnerRepository;
        _redis = redis;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.userId);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.NotFound, Constants.MessageCommon.NotFound, "userId", request.userId ?? "");
        }

        if (user.RoleId == "3")
        {
            double totalMinutes = 0;
            var recentCourses = await _learnerRepository.GetRecentCourseByUserId(request.userId);
            var courseDtos = _mapper.Map<List<CourseProfileDto>>(recentCourses.Select(a => a.Courses));
            foreach (var coursedto in courseDtos)
            {
                var author = await _userRepository.GetById(coursedto.CreatedBy);
                coursedto.Author = author.Username;
            }

            var studyTimes = await _studyTimeRepository.GetStudyTimeByUserId(request.userId);
            foreach (var time in studyTimes)
            {
                totalMinutes += time.StudyTimes;
            }
            var learnerDto = _mapper.Map<LearnerProfileDto>(user);
            learnerDto.TotalDays = studyTimes.Count;
            learnerDto.TotalMinutes = totalMinutes;
            learnerDto.statistics.TotalLearningCourses = await _learnerRepository.CountNumberOfCourseByUserId(request.userId);
            //learnerDto.statistics.TotalLearningTime = await _learnerRepository.TotalLearningTimeByUserId(request.userId);
            learnerDto.RecentCourses = courseDtos;
            learnerDto.learningData = GenerateLearningHeatmap(studyTimes);
            learnerDto.statistics.Rank = await _redis.GetSortSetRankAsync("leaderboard:season1", request.userId);

            return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.GetSuccesfully, learnerDto, "name", learnerDto.Email ?? "");
        }
        else if (user.RoleId == "2") 
        {
            var courses = await _courseRepository.GetCoursesByInstructorIdAsync(user.Id);
            var courseDtos = _mapper.Map<List<CourseProfileDto>>(courses);

            foreach (var coursedto in courseDtos)
            {
                coursedto.Author = user.Username;
            }

            int totalLearners = courses.Sum(c => c.CourseLearners?.Count ?? 0);
            int totalReviews = courses.Sum(c => c.Feedbacks?.Count ?? 0);

            int totalRatings = 0;
            int totalFeedbacks = 0;

            foreach (var course in courses)
            {
                if (course.Feedbacks != null)
                {
                    totalRatings += course.Feedbacks.Sum(f => f.Rating); 
                    totalFeedbacks += course.Feedbacks.Count;            
                }
            }

            double averageReview = totalFeedbacks > 0
                ? (double)totalRatings / totalFeedbacks
                : 0;


            var instructorDto = _mapper.Map<InstructorProfileDto>(user);
            instructorDto.Courses = courseDtos;
            instructorDto.TotalLearners = totalLearners;
            instructorDto.TotalReviews = totalReviews;
            instructorDto.AverageReviews = averageReview;
            if (!string.IsNullOrEmpty(user.AssignToExpertId))
            {
                var expertUser = await _userRepository.GetById(user.AssignToExpertId);
                if (expertUser != null)
                {
                    instructorDto.ExpertName = expertUser.Username;
                }
            }

            return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.GetSuccesfully, instructorDto, "name", instructorDto.Email ?? "");
        }
        else
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, Constants.MessageCommon.InvalidRole, Constants.MessageCommon.InvalidRole, "name", user.RoleId ?? "");
        }
    }

    private List<LearningHeatmap> GenerateLearningHeatmap(IList<StudyTime> studyTimes)
    {
        var heatmap = new List<LearningHeatmap>();
        foreach (var studyTime in studyTimes)
        {
            heatmap.Add(new LearningHeatmap
            {
                Date = studyTime.Date.ToString("yyyy-MM-dd"),
                Count = studyTime.StudyTimes
            });
        }
            
        return heatmap;
    }
}
