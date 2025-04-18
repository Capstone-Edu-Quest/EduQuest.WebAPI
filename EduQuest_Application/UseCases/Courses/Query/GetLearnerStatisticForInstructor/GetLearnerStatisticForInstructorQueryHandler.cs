using AutoMapper;
using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.DTO.Response.Materials;
using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Application.Helper;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Query.GetLearnerStatisticForInstructor
{
	public class GetLearnerStatisticForInstructorQueryHandler : IRequestHandler<GetLearnerStatisticForInstructorQuery, APIResponse>
	{
		private readonly ILearnerRepository _learnerRepository;
		private readonly ICertificateRepository _certificateRepository;
		private readonly ITransactionDetailRepository _transactionDetailRepository;
		private readonly ILessonRepository _lessonRepository;
		private readonly ICourseRepository _courseRepository;
		private readonly IMaterialRepository _materialRepository;
		private readonly ILessonMaterialRepository _lessonMaterialRepository;
		private readonly IQuizAttemptRepository _quizAttemptRepository;
		private readonly IAssignmentAttemptRepository _assignmentAttemptRepository;
		private readonly IMapper _mapper;

		public GetLearnerStatisticForInstructorQueryHandler(ILearnerRepository learnerRepository, 
			ICertificateRepository certificateRepository, 
			ITransactionDetailRepository transactionDetailRepository, 
			ILessonRepository lessonRepository, 
			ICourseRepository courseRepository, 
			IMaterialRepository materialRepository, 
			ILessonMaterialRepository lessonMaterialRepository, 
			IQuizAttemptRepository quizAttemptRepository, 
			IAssignmentAttemptRepository assignmentAttemptRepository, 
			IMapper mapper)
		{
			_learnerRepository = learnerRepository;
			_certificateRepository = certificateRepository;
			_transactionDetailRepository = transactionDetailRepository;
			_lessonRepository = lessonRepository;
			_courseRepository = courseRepository;
			_materialRepository = materialRepository;
			_lessonMaterialRepository = lessonMaterialRepository;
			_quizAttemptRepository = quizAttemptRepository;
			_assignmentAttemptRepository = assignmentAttemptRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetLearnerStatisticForInstructorQuery request, CancellationToken cancellationToken)
		{
			var learners = await _learnerRepository.GetListLearnerOfCourse(request.CourseId);

			//Get list quizId adn assignmentId
			var course = await _courseRepository.GetById(request.CourseId);
			var lessonIds = course.Lessons.Select(x => x.Id).ToList();
			var materialIds = (await _lessonMaterialRepository.GetByListLessonId(lessonIds)).Select(x => x.MaterialId).Distinct().ToList();
			var quizIds = (await _materialRepository.GetMaterialsByType(materialIds, GeneralEnums.TypeOfMaterial.Quiz.ToString())).Select(x => x.QuizId).ToList();
			var assignmentIds = (await _materialRepository.GetMaterialsByType(materialIds, GeneralEnums.TypeOfMaterial.Assignment.ToString())).Select(x => x.AssignmentId).ToList();

			//Get list learnerIds
			var learnerIds = learners.Select(x => x.UserId).Distinct();
			var response = new List<LearnerStatisticForInstructor>();
			foreach(var learnerId in learnerIds)
			{
				var listLessonBasic = new List<LessonBasicResponse>();
				
				var quizAttempts = await _quizAttemptRepository.GetQuizzesAttempts(quizIds, lessonIds, learnerId);
				var assignmentAttempts = await _assignmentAttemptRepository.GetAssignmentAttempts(assignmentIds, lessonIds, learnerId);
				var quizLessonIds = quizAttempts
					.Select(x => x.LessonId)
					.ToList();

				// Lấy danh sách lessonId từ assignmentAttempts
				var assignmentLessonIds = assignmentAttempts
					.Select(x => x.LessonId)
					.ToList();

				// Gộp và lọc trùng
				var distinctLessonIds = quizLessonIds
					.Union(assignmentLessonIds)
					.Distinct()
					.ToList();

				var lessons = (await _lessonRepository.GetByIdsAsync(distinctLessonIds)).OrderBy(x => x.Index);
				foreach(var lesson in lessons)
				{
					//Get quiz and assignment in this lesson
					var quizAttemptThisCourse = quizAttempts.Where(x => x.LessonId == lesson.Id).ToList().OrderBy(x => x.CreatedAt);
					var assignmentAttemptThisCourse = assignmentAttempts.Where(x => x.LessonId == lesson.Id).ToList();

					var quizzes = new List<QuizBasicResponse>();
					var assignments = new List<AssignmentBasicResponse>();

					//Mapping
					foreach (var quizAttempt in quizAttemptThisCourse)
					{
						var quizBasic = _mapper.Map<QuizBasicResponse>(quizAttempt);
						quizzes.Add(quizBasic);
					}

					foreach (var assignmentAttempt in assignmentAttemptThisCourse)
					{
						var assignmentBasic = _mapper.Map<AssignmentBasicResponse>(assignmentAttempt);
						assignments.Add(assignmentBasic);
					}
					var lessonBasic = new LessonBasicResponse
					{
						Id = lesson.Id,
						Name = lesson.Name,
						Quizzes = quizzes,
						Assignments = assignments
					};
					listLessonBasic.Add(lessonBasic);
				}
				var paymentInfo = await _transactionDetailRepository.GetCourseTransactionInfoAsync(request.CourseId, learnerId);
				var certificate = await _certificateRepository.GetByUserIdAndCourseId(request.CourseId, learnerId);
				var statistic = new LearnerStatisticForInstructor
				{
					UserId = learnerId,
					Progress = (decimal)learners.FirstOrDefault(x => x.UserId == learnerId).ProgressPercentage,
					Lessons = listLessonBasic,
					CreatedAt = paymentInfo.CreatedAt,
					Amount = paymentInfo.Amount,
					CertificateId = certificate != null ? certificate.Id : null,
				};
				response.Add(statistic);
			}
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", $"Learner Statistic for Course with ID {request.CourseId}");
		}
	}
}
