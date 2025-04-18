using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Query.GetLearnerOverviewForInstructor
{
	public class GetLearnerOverviewForInstructorQueryHandler : IRequestHandler<GetLearnerOverviewForInstructorQuery, APIResponse>
	{
		private readonly ILearnerRepository _learnerRepository;
		private readonly ICertificateRepository _certificateRepository;
		private readonly ITransactionDetailRepository _transactionDetailRepository;
		private readonly IUserRepository _userRepository;

		public GetLearnerOverviewForInstructorQueryHandler(ILearnerRepository learnerRepository, ICertificateRepository certificateRepository, ITransactionDetailRepository transactionDetailRepository, IUserRepository userRepository)
		{
			_learnerRepository = learnerRepository;
			_certificateRepository = certificateRepository;
			_transactionDetailRepository = transactionDetailRepository;
			_userRepository = userRepository;
		}

		public async Task<APIResponse> Handle(GetLearnerOverviewForInstructorQuery request, CancellationToken cancellationToken)
		{
			var learners = await _learnerRepository.GetListLearnerOfCourse(request.CourseId);

			//Get list learnerIds
			var learnerIds = learners.Select(x => x.UserId).Distinct();
			var response = new List<LearnerOverviewForInstructor>();

			foreach(var learnerId in learnerIds)
			{
				var userInfo = await _userRepository.GetById(learnerId);
				var paymentInfo = await _transactionDetailRepository.GetCourseTransactionInfoAsync(request.CourseId, learnerId);
				var certificate = await _certificateRepository.GetByUserIdAndCourseId(request.CourseId, learnerId);
				var statistic = new LearnerOverviewForInstructor
				{
					UserId = learnerId,
					UserName = userInfo.Username,
					Progress = (decimal)learners.FirstOrDefault(x => x.UserId == learnerId).ProgressPercentage,
					EnrolledDate = paymentInfo.CreatedAt,
					PurchasedAmount = paymentInfo.Amount,
					CertificateId = certificate != null ? certificate.Id : null,
				};
				response.Add(statistic);
			}
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", $"Learner Overview for Course with ID {request.CourseId}");
		}
	}
}
