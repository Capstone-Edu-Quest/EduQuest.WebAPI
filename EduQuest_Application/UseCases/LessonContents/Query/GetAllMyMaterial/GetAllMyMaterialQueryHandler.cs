using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.LessonContents.Query.GetAllMyMaterial
{
	public class GetAllMyMaterialQueryHandler : IRequestHandler<GetAllMyMaterialQuery, APIResponse>
	{
		private readonly IMaterialRepository _materialRepository;
		private readonly IAssignmentRepository _assignmentRepository;
		private readonly IQuizRepository _quizRepository;

		public GetAllMyMaterialQueryHandler(IMaterialRepository materialRepository, IAssignmentRepository assignmentRepository, IQuizRepository quizRepository)
		{
			_materialRepository = materialRepository;
			_assignmentRepository = assignmentRepository;
			_quizRepository = quizRepository;
		}

		public async Task<APIResponse> Handle(GetAllMyMaterialQuery request, CancellationToken cancellationToken)
		{
			var listMaterial = await _materialRepository.GetByUserId(request.UserId, request.Info);
			var listAssignment = await _assignmentRepository.GetByUserId(request.UserId, request.Info);
			var listQuiz = await _quizRepository.GetByUserId(request.UserId, request.Info);
			var response = new
			{
				Videos = new
				{
					Total = listMaterial.Count(m => m.Type == TypeOfMaterial.Video.ToString()),
					Items = listMaterial.Where(m => m.Type == TypeOfMaterial.Video.ToString())
				.Select(m => new
				{
					m.Id,
					m.Title,
					m.Description,
					Duration = m.Duration // Duration for each video
				}).ToList()
				},
				Document = new
				{
					Total = listMaterial.Count(m => m.Type == TypeOfMaterial.Document.ToString()),
					Items = listMaterial.Where(m => m.Type == TypeOfMaterial.Document.ToString())
					.Select(m => new
					{
						m.Id,
						m.Title,
						m.Description,
					})
				},
				Quiz = new
				{
					Total = listQuiz.Count(),
					Items = listQuiz
					.Select(m => new
					{
						m.Id,
						m.Title,
						m.Description,
						m.TimeLimit,
						m.PassingPercentage,
						QuestionCount = m.Questions.Count() // Duration for each video
					}).ToList()
				},
				Assignment = new
				{
					Total = listAssignment.Count(),
					Items = listAssignment
					.Select(m => new
					{
						m.Id,
						m.Title,
						m.Description,
						m.TimeLimit,
						m.AnswerLanguage,
						Language = m!.AnswerLanguage
					})
				}
			};

			return GeneralHelper.CreateSuccessResponse(
				   HttpStatusCode.OK,
				   MessageCommon.GetSuccesfully,
				   response,
				   "name",
				   "Lesson Content"
			);
		}
	}
}
