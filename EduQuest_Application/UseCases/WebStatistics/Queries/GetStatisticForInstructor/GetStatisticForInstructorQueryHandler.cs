using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Application.DTO.Response.HomeStatistic;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.WebStatistics.Queries.GetStatisticForInstructor
{
	public class GetStatisticForInstructorQueryHandler : IRequestHandler<GetStatisticForInstructorQuery, APIResponse>
    {
        private readonly ICourseStatisticRepository _courseStatisticRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILearnerRepository _learnerRepository;
		private readonly IFireBaseRealtimeService _notifcation;

		public GetStatisticForInstructorQueryHandler(ICourseStatisticRepository courseStatisticRepository, 
			ITransactionDetailRepository transactionDetailRepository, 
			ICourseRepository courseRepository, 
			ILearnerRepository learnerRepository, IFireBaseRealtimeService notifcation)
		{
			_courseStatisticRepository = courseStatisticRepository;
			_transactionDetailRepository = transactionDetailRepository;
			_courseRepository = courseRepository;
			_learnerRepository = learnerRepository;
			_notifcation = notifcation;
		}

		public async Task<APIResponse> Handle(GetStatisticForInstructorQuery request, CancellationToken cancellationToken)
		{
			var myCourseIds = (await _courseRepository.GetCourseByUserId(request.UserId)).Select(x => x.Id).Distinct().ToList();
			var totalCourses = myCourseIds.Count();
			var totalLearners = (await _courseStatisticRepository.GetTotalLearnerForInstructor(myCourseIds)).totalLearner;
			var avgReview = (await _courseStatisticRepository.GetTotalLearnerForInstructor(myCourseIds)).avgRating;
			var totalRevenue = await _transactionDetailRepository.GetTotalRevenueByInstructorId(request.UserId);
			var topCourseLearner = (await _learnerRepository.GetTopCourseLearner(myCourseIds)).Take(4);
			var listNotification = await _notifcation.GetNotificationsAsync(request.UserId);

			var response = new StatisticForInstructor
			{
				TotalCourses = totalCourses,
				TotalLearners = totalLearners,
				AverageReviews = avgReview,
				TotalRevenue = totalRevenue,
				TopCourses = topCourseLearner.ToList(),
				Notification = listNotification
			};

			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", $"Statis for User Id {request.UserId}");
		}
	}
}
