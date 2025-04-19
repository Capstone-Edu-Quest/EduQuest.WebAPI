using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Queries

{
	public class GetCourseCreatedByMeQueryHandler : IRequestHandler<GetCourseCreatedByMeQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly ICourseStatisticRepository _courseStatisticRepository;
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;

		public GetCourseCreatedByMeQueryHandler(ICourseRepository courseRepository, ICourseStatisticRepository courseStatisticRepository, IMapper mapper, IUserRepository userRepository)
		{
			_courseRepository = courseRepository;
			_courseStatisticRepository = courseStatisticRepository;
			_mapper = mapper;
			_userRepository = userRepository;
		}

		public async Task<APIResponse> Handle(GetCourseCreatedByMeQuery request, CancellationToken cancellationToken)
		{
			var apiResponse = new APIResponse();
			var courseList = await _courseRepository.GetCourseByUserId(request.UserId);

			var listCourseResponse = _mapper.Map<List<OverviewCourseResponse>>(courseList);
			foreach (var course in listCourseResponse)
			{

				var user = await _userRepository.GetById(course.CreatedBy);
				course.Author = user!.Username!;

				var courseSta = await _courseStatisticRepository.GetByCourseId(course.Id);
				if (courseSta != null)
				{
					course.TotalLesson = courseSta.TotalLesson;
					course.TotalReview = courseSta.TotalReview;
					course.Rating = courseSta.Rating;
					course.TotalTime = courseSta.TotalTime;
				}
			}
			return apiResponse = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, listCourseResponse, "name", $"course created by me");
		} 
	}
}
