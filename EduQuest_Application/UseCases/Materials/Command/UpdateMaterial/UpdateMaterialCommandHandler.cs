using AutoMapper;
using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Materials.Command.UpdateMaterial
{
	public class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand, APIResponse>
	{
		private readonly IMaterialRepository _materialRepository;
		private readonly ICourseRepository _courseRepository;
		private readonly ISystemConfigRepository _systemConfigRepository;
		private readonly IAssignmentRepository _assignmentRepository;
		private readonly IQuizRepository _quizRepository;
		private readonly IQuestionRepository _questionRepository;
		private readonly IAnswerRepository _answerRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UpdateMaterialCommandHandler(IMaterialRepository materialRepository, ICourseRepository courseRepository, ISystemConfigRepository systemConfigRepository, IAssignmentRepository assignmentRepository, IQuizRepository quizRepository, IQuestionRepository questionRepository, IAnswerRepository answerRepository, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_materialRepository = materialRepository;
			_courseRepository = courseRepository;
			_systemConfigRepository = systemConfigRepository;
			_assignmentRepository = assignmentRepository;
			_quizRepository = quizRepository;
			_questionRepository = questionRepository;
			_answerRepository = answerRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
		{
			var isOwner = await _materialRepository.IsOwnerThisMaterial(request.UserId, request.Material.Id);
			if (isOwner == false)
			{
				return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Not owner", MessageCommon.NotOwner, "name", "material");
			}

			var material = await _materialRepository.GetMataterialQuizAssById(request.Material.Id);
			var value = await _systemConfigRepository.GetByName(material.Type!);

			material.Title = request.Material.Title;
			material.Description = request.Material.Description;

			switch (Enum.Parse(typeof(TypeOfMaterial), material.Type))
			{
				case TypeOfMaterial.Document:
					material = await ProcessDocumentMaterialAsync(request.Material, value, material);
					break;

				case TypeOfMaterial.Quiz:
					material = await ProcessQuizMaterialAsync(request.Material, value, material);
					break;

				case TypeOfMaterial.Video:
					material = await ProcessVideoMaterialAsync(request.Material, value, material);
					break;

				case TypeOfMaterial.Assignment:
					material = await ProcessAssignmentMaterialAsync(request.Material, value, material);
					break;

				default:
					break;
			}

			await _materialRepository.Update(material);

			var result = await _unitOfWork.SaveChangesAsync() > 0;

			return new APIResponse
			{
				IsError = !result,
				Payload = result ? material : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.UpdateFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.UpdateSuccesfully : MessageCommon.UpdateFailed,
					values = new Dictionary<string, string> { { "name", "learning material" } }
				}
			};
		}

		private async Task<Material> ProcessDocumentMaterialAsync(UpdateLearningMaterialRequest item, SystemConfig config, Material material)
		{
			material.Content = item.Content;
			material.Duration = (int)config.Value!;
			return material;
		}

		private async Task<Material> ProcessQuizMaterialAsync(UpdateLearningMaterialRequest item, SystemConfig config, Material material)
		{
			// Update thời lượng
			material.Duration = (int)(item.Quiz!.TimeLimit! * (config?.Value ?? 1));

			// Lấy quiz hiện tại và cập nhật lại
			var quiz = await _quizRepository.GetQuizById(material.QuizId);
			if (quiz != null)
			{
				

				// Xoá toàn bộ answer cũ
				var questions = quiz.Questions.ToList();
				foreach (var question in questions)
				{

                    var listAnswer = question.Answers.ToList();
                    _answerRepository.DeleteRange(listAnswer);
                    await _unitOfWork.SaveChangesAsync();
                }

				// Xoá toàn bộ question cũ
				_questionRepository.DeleteRange(questions);

                // Cập nhật lại quiz
                _mapper.Map(item.Quiz, quiz);
                await _quizRepository.Update(quiz);

                await _unitOfWork.SaveChangesAsync();
			}

			// Tạo mới câu hỏi
			var newQuestions = _mapper.Map<List<Question>>(item.Quiz.Questions);
			foreach (var question in newQuestions)
			{
				question.Id = Guid.NewGuid().ToString();
				question.QuizId = quiz!.Id;
			}
			await _questionRepository.CreateRangeAsync(newQuestions);

			// Tạo mới câu trả lời
			var allAnswers = new List<Answer>();
			foreach (var questionRequest in item.Quiz.Questions)
			{
				var targetQuestion = newQuestions.First(q => q.QuestionTitle == questionRequest.QuestionTitle);
				var answers = _mapper.Map<List<Answer>>(questionRequest.Answers);

				foreach (var answer in answers)
				{
					answer.Id = Guid.NewGuid().ToString();
					answer.QuestionId = targetQuestion.Id;
				}

				allAnswers.AddRange(answers);
			}
			await _answerRepository.CreateRangeAsync(allAnswers);

			await _unitOfWork.SaveChangesAsync();
			return material;
		}


		private async Task<Material> ProcessVideoMaterialAsync(UpdateLearningMaterialRequest item, SystemConfig config, Material material)
		{
			material.Duration = item.Video!.Duration;
			material.UrlMaterial = item.Video.UrlMaterial;
			material.Thumbnail = item.Video.Thumbnail;

			if (item.Quiz != null)
			{
				await ProcessQuizMaterialAsync(item, config, material);
			}
			return material;
		}


		private async Task<Material> ProcessAssignmentMaterialAsync(UpdateLearningMaterialRequest item, SystemConfig config, Material material)
		{
			var assignment = _mapper.Map<Assignment>(item.Assignment);
			assignment.Id = material.Assignment?.Id ?? Guid.NewGuid().ToString();

			material.AssignmentId = assignment.Id;
			material.Assignment = assignment;
			material.Duration = (int)(item.Assignment!.TimeLimit! * config.Value!);

			await _assignmentRepository.Update(assignment);
			return material;
		}
	}
}
