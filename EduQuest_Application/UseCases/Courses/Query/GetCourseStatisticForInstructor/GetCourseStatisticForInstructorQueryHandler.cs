using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Query.GetCourseStatisticForInstructor
{
	public class GetCourseStatisticForInstructorQueryHandler : IRequestHandler<GetCourseStatisticForInstructorQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly ILearnerRepository _learnerRepository;
		private readonly IFeedbackRepository _feedbackRepository;

		public GetCourseStatisticForInstructorQueryHandler(ICourseRepository courseRepository, ILearnerRepository learnerRepository, IFeedbackRepository feedbackRepository)
		{
			_courseRepository = courseRepository;
			_learnerRepository = learnerRepository;
			_feedbackRepository = feedbackRepository;
		}

		public async Task<APIResponse> Handle(GetCourseStatisticForInstructorQuery request, CancellationToken cancellationToken)
		{
			var response = new APIResponse();
			var courseList = await _courseRepository.GetCourseByUserId(request.UserId);
			var result = new CourseStatisticForInstructor();
			var listCourseId = courseList.Select(x => x.Id).Distinct().ToList();

			result.CoursesEnroll = await _learnerRepository.GetMyCoursesEnrollOverTimeAsync(listCourseId);
			result.CoursesReview = await _feedbackRepository.GetMyCoursesRatingOverTimeAsync(listCourseId);
			result.LearnerStatus = await _learnerRepository.GetLearnerStatusAsync(listCourseId);
			result.TopCourseInfo  = await _learnerRepository.GetTop3CoursesAsync(listCourseId);

			return response = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, result, "name", $"course overview");
		}
	}
}
