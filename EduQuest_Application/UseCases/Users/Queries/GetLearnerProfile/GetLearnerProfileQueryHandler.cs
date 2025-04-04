using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using AutoMapper;
using EduQuest_Application.DTO.Response.Profiles;
using EduQuest_Application.DTO.Response.Profiles.TotalCourse;
using EduQuest_Application.DTO.Response.Profiles.Heatmap;
using EduQuest_Domain.Entities;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Application.Abstractions.Redis;

namespace EduQuest_Application.UseCases.Users.Queries.GetLearnerProfile;

public class GetLearnerProfileQueryHandler : IRequestHandler<GetLearnerProfileQuery, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IStudyTimeRepository _studyTimeRepository;
    private readonly ILearnerRepository _learnerRepository;
    private readonly IRedisCaching _redis;
    private readonly IMapper _mapper;

    public GetLearnerProfileQueryHandler(IUserRepository userRepository, IStudyTimeRepository studyTimeRepository, ILearnerRepository learnerRepository, IRedisCaching redis, IMapper mapper)
    {
        _userRepository = userRepository;
        _studyTimeRepository = studyTimeRepository;
        _learnerRepository = learnerRepository;
        _redis = redis;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetLearnerProfileQuery request, CancellationToken cancellationToken)
    {
        var totalMinutes = 0;
        // 1️⃣ Lấy thông tin user
        var user = await _userRepository.GetUserById(request.userId);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.NotFound, Constants.MessageCommon.NotFound, "name", request.userId ?? "");

        }

        var recentCourses = await _learnerRepository
            .GetRecentCourseByUserId(request.userId);

        var courseDtos = _mapper.Map<List<CourseProfileDto>>(recentCourses.Select(a => a.Courses));

        var studyTimes = await _studyTimeRepository.GetStudyTimeByUserId(request.userId);

        foreach (var time in studyTimes)
        {
            totalMinutes += time.StudyTimes;
        }
        var learningData = GenerateLearningHeatmap(studyTimes);

        var learnerProfileDto = _mapper.Map<LearnerProfileDto>(user);
        learnerProfileDto.TotalDays = studyTimes.Count();
        learnerProfileDto.TotalMinutes = totalMinutes;
        learnerProfileDto.statistics.TotalLearningCourses = await _learnerRepository.CountNumberOfCourseByUserId(request.userId);
        learnerProfileDto.RecentCourses = courseDtos;
        learnerProfileDto.learningData = learningData;
        learnerProfileDto.statistics.Rank = await _redis.GetSortSetRankAsync("leaderboard:season1", request.userId);

        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.GetSuccesfully, learnerProfileDto, "name", learnerProfileDto.Email ?? "");

    }

    private List<LearningHeatmap> GenerateLearningHeatmap(IList<StudyTime> studyTimes)
    {
        var heatmap = new List<LearningHeatmap>();
        var currentYear = DateTime.UtcNow.Year;

        for (int day = 1; day <= 365; day++)
        {
            var date = new DateTime(currentYear, 1, 1).AddDays(day - 1);
            var studyTime = studyTimes.FirstOrDefault(s => s.Date.Date == date.Date);

            heatmap.Add(new LearningHeatmap
            {
                Date = date.ToString("yyyy-MM-dd"),
                Count = studyTime != null ? studyTime.StudyTimes : 0
            });
        }

        return heatmap;
    }
}
