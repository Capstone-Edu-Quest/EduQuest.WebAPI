using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Query.GetCourseStudying
{
	public class GetCourseStudyingQueryHandler : IRequestHandler<GetCourseStudyingQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly ILearnerRepository _learnerRepository;
		private readonly IUserRepository _userRepository;
		private readonly ICourseStatisticRepository _courseStatisticRepository;
		private readonly IMapper _mapper;

		public GetCourseStudyingQueryHandler(ICourseRepository courseRepository, ILearnerRepository learnerRepository, IUserRepository userRepository, ICourseStatisticRepository courseStatisticRepository, IMapper mapper)
		{
			_courseRepository = courseRepository;
			_learnerRepository = learnerRepository;
			_userRepository = userRepository;
			_courseStatisticRepository = courseStatisticRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetCourseStudyingQuery request, CancellationToken cancellationToken)
		{
			var coursesLearner = (await _learnerRepository.GetCoursesByUserId(request.UserId))
				.OrderByDescending(x => x.CreatedAt)
				.ToList();

			var listCourseId = coursesLearner.Select(x => x.CourseId).ToList();

			var listCourse = await _courseRepository.GetByListIds(listCourseId);

			listCourse = listCourseId
				.Select(id => listCourse.FirstOrDefault(c => c.Id == id))
				.Where(c => c != null && c.Status == GeneralEnums.StatusCourse.Public.ToString())
				.ToList();

			var listCourseResponse = _mapper.Map<List<CourseSearchResponse>>(listCourse);

			var courseIds = listCourseResponse.Select(c => c.Id).ToList();
			var learners = await _learnerRepository.GetByUserIdAndCourseIdsAsync(request.UserId, courseIds);
			var learnerDict = learners.ToDictionary(x => x.CourseId, x => x.ProgressPercentage);

			foreach (var course in listCourseResponse)
			{
                //var user = await _userRepository.GetById(course.CreatedBy);
                //course.Author = user!.Username!;

                //var courseSta = await _courseStatisticRepository.GetByCourseId(course.Id);
                //if (courseSta != null)
                //{
                //	course.TotalLesson = (int)courseSta.TotalLesson;
                //	course.TotalReview = (int)courseSta.TotalReview;
                //	course.Rating = (int)courseSta.Rating;
                //	course.TotalTime = (int)courseSta.TotalTime;
                //}

                //var courseLeanrer = await _learnerRepository.GetByUserIdAndCourseId(request.UserId, course.Id);
                //if (courseLeanrer != null)
                //{
                //	course.ProgressPercentage = courseLeanrer.ProgressPercentage;
                //}
                //else
                //{
                //	course.ProgressPercentage = null;
                //}

                if (learnerDict.TryGetValue(course.Id, out var progress))
                    course.ProgressPercentage = progress;
            }
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, listCourseResponse, "name", "course studying");
		}
	}
}
